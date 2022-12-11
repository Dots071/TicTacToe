using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Controllers.Players;
using Utils;
using Models;


namespace Controllers.GameGrid
{

    /// <summary>
    /// This is the longest script in the project, currently responsible for updating the grid date and the turn management. I could definitely split it to at least 2 managers.
    /// There is also depandancy to decouple with the player and tiles.
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        // A reference to get all TileModel components and organize them in a matrix.
        [SerializeField] private GameObject _tileContainer;

        // game and grid models for getting relevant public data.
        [SerializeField] private GameModelScriptableObj _gameModel;
        [SerializeField] private GridModelScriptableObj _gridModel;

        // An array that will contain the 2 players in the game, player must implement the ICanPlay interface. This helps us support other game modes like network PvP (match making).
        private ICanPlay[] _players = new ICanPlay[2];
        // a new tile matrix set according to the tile's location.
        private TileModel[,] _tilesInGrid;
        // a win checker service.
        private WinChecker _winChecker = new WinChecker();

        private void OnEnable()
        {
            if (GameEvents.isInitialized == false)
                Debug.LogError("GridManager: Can't find GameEvents' instance.");

            GameEvents.Instance.TurnTimerEnded += OnTurnTimerEnded;
            GameEvents.Instance.UndoButtonPressed += OnUndoButtonPressed;
        }

        private void OnDisable()
        {
            if (GameEvents.isInitialized)
            {
                GameEvents.Instance.TurnTimerEnded -= OnTurnTimerEnded;
                GameEvents.Instance.UndoButtonPressed -= OnUndoButtonPressed;
            }
            ResetModelData();
        }

        private void Start()
        {
            // set initial grid game data.
            _gridModel.CurrentPlayerIndex = 0;
            _gridModel.GridSize = 3;
            _gridModel.TurnIndex = 0;
            // set the grid size.
            _tilesInGrid = new TileModel[_gridModel.GridSize, _gridModel.GridSize];
            SetTilesInGrid();
            SetPlayersForGame();
            RandomizeWhoPlayesFirst();
            StartNextTurn();
        }
        // fills out the matrix with TileModels according to their location.
        private void SetTilesInGrid()
        {
            Tile[] tilesInScene = _tileContainer.GetComponentsInChildren<Tile>();

            if (tilesInScene.Length != Mathf.Pow(_gridModel.GridSize, 2))
                Debug.LogError("Grid: tiles array is too short, check the amount of tiles in the inspector.");

            // Organizes the tiles in a 2D array, according to their indexs.
            foreach (Tile tile in tilesInScene)
            {
                _tilesInGrid[tile.TileModel.RowIndex, tile.TileModel.ColumnIndex] = tile.TileModel;
            }
        }
        // fill the _players array according to the game mode that was chosen.
        private void SetPlayersForGame()
        {
            _players[0] = new PlayerController();

            switch (_gameModel.ChosenGameMode)
            {
                case Common.GameMode.TwoPlayers:
                    _players[1] = new PlayerController();
                    break;

                case Common.GameMode.VsComputer:
                    _players[1] = new ComputerPlayer();
                    break;

                default:
                    break;
            }
        }

        // invokes the pre-turn game state, waits for a few seconds and activates the relevant player to start the turn.
        private async void StartNextTurn()
        {
            GameEvents.Instance.InvokeGameStateChanged(Common.GameState.PreTurnTransition);

            await Task.Delay(_gridModel.TurnTransitionDelay);

            if(GameEvents.isInitialized)
                GameEvents.Instance.InvokeGameStateChanged(Common.GameState.TurnStarted);

            // with play turn - passing a callback to activate when move is played.
            _players[_gridModel.CurrentPlayerIndex].PlayTurn(GetPlayerMoveCallback, _gridModel.MovesLog);

        }

        private void GetPlayerMoveCallback(int row, int column)
        {
            UpdateMoveInGrid(row, column);
            bool isWin = _winChecker.CheckForWin(_gridModel.TurnIndex, row, column, _tilesInGrid);
            CheckEndTurnState(isWin);

        }

        // checks if the game has ended.
        private void CheckEndTurnState(bool isWin)
        {
            if (isWin == true)
            {
                EndGame(_gridModel.CurrentPlayerIndex == 0 ? Common.EndRoundState.Player1Win : Common.EndRoundState.Player2Win);
            }
            else
            {
                // Check if draw
                if (_gridModel.MovesLog.Count == _tilesInGrid.Length)
                {
                    EndGame(Common.EndRoundState.Draw);
                }
                else
                {
                    SwitchCurrentPlayer();
                    _gridModel.TurnIndex++;
                    StartNextTurn();
                }
            }
        }

        //turnMan
        private void SwitchCurrentPlayer()
        {
            // Switch between player1 to player2.
            _gridModel.CurrentPlayerIndex++;
            if (_gridModel.CurrentPlayerIndex > 1)
                _gridModel.CurrentPlayerIndex = 0;
        }
        private void OnUndoButtonPressed()
        {
            if (_gridModel.MovesLog.Count == 0 || _gameModel.CurrentGameState != Common.GameState.TurnStarted)
                return;

            //stop the current player from playing.
            _players[_gridModel.CurrentPlayerIndex].CancelTurn();

            UndoLastMove();

            StartNextTurn();

        }
        private void UndoLastMove()
        {
            //empty the last marked tile.
            int lastLogIndex = _gridModel.MovesLog.Count - 1;
            int row = _gridModel.MovesLog[lastLogIndex][0];
            int column = _gridModel.MovesLog[lastLogIndex][1];
            GameEvents.Instance.InvokeUpdateTileMark(row, column, Common.TileMark.Empty);

            //remove last move log.
            _gridModel.MovesLog.RemoveAt(lastLogIndex);
            
            _gridModel.TurnIndex--;

        }

        private void UpdateMoveInGrid(int row, int column)
        {
            var currentMark = _gridModel.TurnIndex % 2 == 0 ? Common.TileMark.X : Common.TileMark.O;

            _gridModel.MovesLog.Add(new int[2] { row, column });

            GameEvents.Instance.InvokeUpdateTileMark(row, column, currentMark);
        }


        private void RandomizeWhoPlayesFirst()
        {
            // returns 0 or 1 to set the player that will go first.
            _gridModel.CurrentPlayerIndex = Random.Range(0, 2);
        }

        // called when the turn time countdown has ended.
        private void OnTurnTimerEnded()
        {
            _players[_gridModel.CurrentPlayerIndex].CancelTurn();
            EndGame(Common.EndRoundState.TimesUp);
        }

        private void EndGame(Common.EndRoundState endState)
        {
            _gameModel.CurrentEndState = endState;
            switch (endState)
            {
                case Common.EndRoundState.Player1Win:
                    Debug.Log("Player 1 has won this round!");
                    GameEvents.Instance.InvokeGameStateChanged(Common.GameState.GameEnded);
                    break;

                case Common.EndRoundState.Player2Win:
                    Debug.Log("Player 2 has won this round!");
                    GameEvents.Instance.InvokeGameStateChanged(Common.GameState.GameEnded);
                    break;

                case Common.EndRoundState.Draw:
                    Debug.Log("Game ended with a draw");
                    GameEvents.Instance.InvokeGameStateChanged(Common.GameState.GameEnded);
                    break;

                case Common.EndRoundState.TimesUp:
                    Debug.Log("Times up!");
                    GameEvents.Instance.InvokeGameStateChanged(Common.GameState.GameEnded);
                    break;

                default:
                    break;
            }
        }

        // called on disable.
        private void ResetModelData()
        {
            _gameModel.CurrentEndState = Common.EndRoundState.None;
            _gridModel.MovesLog.Clear();
        }
    }
}


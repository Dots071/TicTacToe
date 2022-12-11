using UnityEngine.UI;
using UnityEngine;
using Utils;
using Models;
using DG.Tweening;

namespace Views
{
    /*  SUMMARY
     *  Responsible of handeling all UI elements in the game scene.
    */
    public class HudView : MonoBehaviour
    {
        [SerializeField] private Text _nextTurnText;
        [SerializeField] private GameObject _undoButton;

        // get game data from the Scriptable Objects' models.
        [SerializeField] private GameModelScriptableObj _gameModel;
        [SerializeField] private GridModelScriptableObj _gridModel;


        //private int _turnTransitionDelay = 2000;

        private void OnEnable()
        {
                GameEvents.Instance.GameStateChanged += OnGameStateChanged;

            // Activates or deactivates the undo button according to the game mode. 
            _undoButton.SetActive(_gameModel.ChosenGameMode == Common.GameMode.VsComputer);

        }
        private void OnDisable()
        {
            if(GameEvents.isInitialized)
                GameEvents.Instance.GameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(Common.GameState state)
        {
            if(state == Common.GameState.PreTurnTransition)
            {
                ShowWhosTurnIsIt();
            }
        }

        // Display next player's turn message.
        private  void ShowWhosTurnIsIt()
        {
            var playerName = GetCurrentPlayersName();
            _nextTurnText.text = playerName + "'s turn!";

            _nextTurnText.CrossFadeAlpha(1, 0, true);
            _nextTurnText.transform.DOScale(1.5f, 2).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
            {
                _nextTurnText.CrossFadeAlpha(0, 0, true);
                _nextTurnText.transform.DOScale(1, 0);
            });
        }

        // Returns the string of the current player's name.
        private string GetCurrentPlayersName()
        {
            switch (_gameModel.ChosenGameMode)
            {
                case Common.GameMode.TwoPlayers:
                    return "Player" + (_gridModel.CurrentPlayerIndex + 1);

                case Common.GameMode.VsComputer:
                    return _gridModel.CurrentPlayerIndex == 0 ? "Player1" : "Computer";

                default:
                    return "Your";
            }
        }
    }
}

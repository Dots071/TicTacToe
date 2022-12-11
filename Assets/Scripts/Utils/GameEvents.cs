
using System;

namespace Utils
{

    /* SUMMARY
     * The GameEvent singleton is an mediator between most of the Controllers and Views in the game. 
     * To invoke events the other componenets use the Invoke + "name of event" functions so all events are invoked in this script, which helps us to lower the amount of dependancy.
     * 
     * On disable, the class unsubscribes all of the event listeners.
     */
    public class GameEvents : Singleton<GameEvents>
    {
        // Invoke to change the game state.
        public event Action<Common.GameState> GameStateChanged;
        // Start button pressed on the main menu, passing which game mode was chosen (PvP, PvComp..).
        public event Action<Common.GameMode> MainStartButtonPressed;
        // Invoked from each tile, passing the data to the relevant player. Might be more efficant to get the click data from one source and divide by position.
        public event Action<int, int> TilePressed;
        // Updates the relevant tile which mark to change to.
        public event Action<int, int, Common.TileMark> UpdateTileMark;
        // happens only on vs computer mode, undo the last player move.
        public event Action UndoButtonPressed;
        // Invoked by the timer when countdown finished and no move was made.
        public event Action TurnTimerEnded;
        // Updates when Restart was chosen.
        public event Action RestartGame;


        private void OnDisable()
        {
            UnsubscribeAllListeners();
        }

        // all invoke events methods:
        public void InvokeMainStartButtonPressed(Common.GameMode gameMode)
        {
            MainStartButtonPressed?.Invoke(gameMode);
        }
        public void InvokeGameStateChanged(Common.GameState gameState)
        {
            GameStateChanged?.Invoke(gameState);
        }

        public void InvokeTilePressed(int row, int column)
        {
            TilePressed?.Invoke(row, column);
        }

        public void InvokeUpdateTileMark(int row, int column, Common.TileMark mark)
        {
            UpdateTileMark?.Invoke(row, column, mark);
        }

        public void InvokeTurnTimerEnded()
        {
            TurnTimerEnded?.Invoke();
        }

        public void InvokeUndoButtonPressed()
        {
            UndoButtonPressed?.Invoke();
        }

        public void InvokeRestartGame()
        {
            RestartGame?.Invoke();
        }

        // empty all subscribers lists.
        private void UnsubscribeAllListeners()
        {
            TurnTimerEnded = delegate { };
            UndoButtonPressed = delegate { };
            RestartGame = delegate { };
            TilePressed = delegate { };
            UpdateTileMark = delegate { };
            MainStartButtonPressed = delegate { };
            GameStateChanged = delegate { };
        }

    }
}


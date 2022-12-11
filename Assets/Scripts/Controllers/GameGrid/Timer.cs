using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;
using Utils;
using System;


/*  SUMMARY
 *  The timer is responsible to limit each turn to the amount set in _secondsToCount (default 5).
 *  When countdown finished, if no move was played, the timer invokes the TurnTimerEnded event.
 */
namespace Controllers.GameGrid
{
    public class Timer : MonoBehaviour
    {
        // appears on the top of the game view, showing the countdown.
        [SerializeField] private Text _timerText;

        private int _secondsToCount = 5;
        // in case a move was played, or gameState was changed.
        bool _timerCanceled = false;


        private void OnEnable()
        {
            GameEvents.Instance.GameStateChanged += OnGameStateChanged;
            GameEvents.Instance.RestartGame += OnRestartGame;
        }

        private void OnDisable()
        {
            if (GameEvents.isInitialized)
            {
                GameEvents.Instance.GameStateChanged -= OnGameStateChanged;
                GameEvents.Instance.RestartGame += OnRestartGame;
            }

            _timerCanceled = true;
        }

        private void Start()
        {   // prepare text for fade in.
            _timerText.CrossFadeAlpha(0,0,true);
        }



        private void OnGameStateChanged(Common.GameState state)
        {
            switch (state)
            {
                case Common.GameState.PreTurnTransition: // Move made, starting a new turn.
                    CancelTimer();
                    break;

                case Common.GameState.TurnStarted:
                    StartTimer();
                    break;

                case Common.GameState.GameEnded:
                    CancelTimer();
                    break;
            }
        }
        private void OnRestartGame()
        {
            CancelTimer();
        }

        private async void StartTimer()
        {
            _timerCanceled = false;

            int countDown = _secondsToCount;
            _timerText.CrossFadeAlpha(1, 0, true);

            // loop until countdown is 0 or timer was canceled.
            while (_timerCanceled == false)
            {
                _timerText.text = "Timer: 0" + countDown;
                await Task.Delay(1000);
                countDown--;

                if(countDown <= 0 && _timerCanceled == false)
                {
                    CancelTimer();
                    GameEvents.Instance.InvokeTurnTimerEnded();
                }
            }
        }

        private void CancelTimer()
        {
            _timerText.CrossFadeAlpha(0, 0, true);
            _timerCanceled = true;
        }
    }
}

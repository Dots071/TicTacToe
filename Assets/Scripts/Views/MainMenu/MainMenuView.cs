
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Views
{
    /*  SUMMARY
     *  responsible for handling all UI elements on the main menu screen, supports easy adding game modes.
     */
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Toggle _twoPlayersGameMode;
        [SerializeField] private Toggle _vsComputerGameMode;
        [SerializeField] private Button _startButton;

        private bool _startButtonWasClicked;


        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _startButtonWasClicked = false;

        }
        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);

        }


        public void OnStartButtonClicked()
        {
            if (_startButtonWasClicked)
                return;

            _startButtonWasClicked = true;
            Common.GameMode gameMode;

            if (_twoPlayersGameMode.isOn)
            {
                gameMode = Common.GameMode.TwoPlayers;
            }
            else if (_vsComputerGameMode.isOn)
            {
                gameMode = Common.GameMode.VsComputer;
            }
            else
            {
                Debug.LogWarning("MainMenuView: all toggles are off.");
                return;
            }

            // Invoke start game event.
            GameEvents.Instance.InvokeMainStartButtonPressed(gameMode);

        }
    }
}

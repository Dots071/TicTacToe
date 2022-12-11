
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utils;
using Models;

namespace Views
{
/*  SUMMARY
 *  EndGameView is responsible for handling all UI elements on the game ended screen.
 */
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] private Text _headerText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;

        [SerializeField] private GameModelScriptableObj _gameModel;
        [SerializeField] private GridModelScriptableObj _gridModel;


        private int _introDuration = 1;

        private void Start()
        {
            transform.DOScale(0, 0);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            DisplayScreen();
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
        }

        private void OnMainMenuButtonClicked()
        {
            GameEvents.Instance.InvokeGameStateChanged(Common.GameState.MainMenu);
        }

        private void OnRestartButtonClicked()
        {
            GameEvents.Instance.InvokeRestartGame();
        }

        private void DisplayScreen()
        {    
            _headerText.text = GetHeaderText();
            transform.DOScale(1, _introDuration);
        }

        private string GetHeaderText()
        {
            switch (_gameModel.CurrentEndState)
            {
                case Common.EndRoundState.Player1Win:
                    return "Player 1 Wins!";

                case Common.EndRoundState.Player2Win:
                    return _gameModel.ChosenGameMode == Common.GameMode.TwoPlayers ? "Player 2 Wins!" : "Computer Wins!";

                case Common.EndRoundState.Draw:
                    return "It's a draw!";

                case Common.EndRoundState.TimesUp:
                    return _gridModel.CurrentPlayerIndex == 0 ? "Player 1 Wins!" : "Computer Wins!";

                default:
                    return "Game Finished!";
            }
        }
    }
}

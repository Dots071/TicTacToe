
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Views
{
    public class UndoBtnView : MonoBehaviour
    {
        // I prefer to refrence the button in the editor, as GetComponent is very resource expensive.
        [SerializeField] private Button _undoBtn;

        private bool _isButtonClicked = false;
        //        private int _clickDelay = 500;

        private void Start()
        {
            _undoBtn.onClick.AddListener(OnUndoButtonClick);
            GameEvents.Instance.GameStateChanged += OnGameStateChanged;
        }



        private void OnDisable()
        {
            _undoBtn.onClick.RemoveListener(OnUndoButtonClick);
            if (GameEvents.isInitialized)
                GameEvents.Instance.GameStateChanged -= OnGameStateChanged;
        }

        private void OnUndoButtonClick()
        {
            if (_isButtonClicked == false)
            {
                // Create a time period that the user can't press the button to prevent click abuse.
                DisableButton(true);

                Debug.Log("Undo button clicked!");
                if (GameEvents.isInitialized)
                    GameEvents.Instance.InvokeUndoButtonPressed();
            }
        }

        private void OnGameStateChanged(Common.GameState state)
        {
            if (state == Common.GameState.TurnStarted)
                DisableButton(false);
        }

        private void DisableButton(bool disable)
        {
            _isButtonClicked = disable;
            _undoBtn.interactable = !disable;
        }

    }
}



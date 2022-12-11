using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Models;

/*  ASSIGNMNET SUMMARY - Start from here.
 *  First of all, thank you for taking the time to review this project, I tried my best to make it readable and clean.
 *  
 *  Planning the game design, I decided to use the MVC pattern with the State-pattern, creating a singleton GameEvents script that all events subscribe to and are invoked from.
 *  The reason I chose this pattern is to reduce dependancy and coupling, though there is still a few scripts to uncouple and spilt to smaller pieces.
 *  The downsides are that debugging and unit-testing can be harder in this way, also events have a small delay so might be difficult working with sounds later on.
 *  
 *  I thought at start to use Zenject and DI, but I wasn't sure you worked with it and I never used it before myself.
 *  
 *  Resource Usage - to improve performace I did the following: 
 *  1) Reduce the amount of MonoBehaviour scripts.
 *  2) Used multiple scenes.
 *  3) Did direct referencing via the Editor, and not using GetComponent or other similar expensive Unity methods.
 *  4) Used Scriptable Objects for data.
 *  
 *  GAME MANAGER SUMMARY
 *  Is responsible for handeling the game state & scene management (might be better to split the code to SceneManager and GameManager).
 *  
 */

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        public enum GameScenes
        {
            UI = 1,
            GAME = 2,
            ENDGAME = 3
        }

        [SerializeField] private GameModelScriptableObj _gameModel;

        private List<AsyncOperation> _loadOperations = new List<AsyncOperation>();


        private void Start()
        {
            GameEvents.Instance.GameStateChanged += OnGameStateChanged;
            GameEvents.Instance.MainStartButtonPressed += OnStartBtnPressed;
            GameEvents.Instance.RestartGame += OnRestartGamePressed;

            _gameModel.CurrentGameState = Common.GameState.MainMenu;
            LoadScene(GameScenes.UI);
        }

        private void OnDisable()
        {
            if (GameEvents.isInitialized)
            {
                GameEvents.Instance.GameStateChanged -= OnGameStateChanged;
                GameEvents.Instance.MainStartButtonPressed -= OnStartBtnPressed;
                GameEvents.Instance.RestartGame -= OnRestartGamePressed;
            }
        }

        private void OnRestartGamePressed()
        {
            // remove active game scene  and reload a new one, didn't have time to add fade in/out, but it is definitely neccassary.
            UnLoadScene(GameScenes.GAME);

            if (_gameModel.CurrentGameState == Common.GameState.GameEnded)
                UnLoadScene(GameScenes.ENDGAME);

            LoadScene(GameScenes.GAME);
        }

        // This is the start button in the main menu, after pressing the game starts in the gameMode that was chosen.
        private void OnStartBtnPressed(Common.GameMode gameMode)
        {
            _gameModel.ChosenGameMode = gameMode;
            GameEvents.Instance.InvokeGameStateChanged(Common.GameState.GameStarting);
        }

        private void OnGameStateChanged(Common.GameState state)
        {
            Debug.Log("GameState changed to: " + state);
            _gameModel.CurrentGameState = state;
            switch (state)
            {
                case Common.GameState.GameStarting:
                    UnLoadScene(GameScenes.UI);
                    LoadScene(GameScenes.GAME);
                    break;

                case Common.GameState.GameEnded:
                    LoadScene(GameScenes.ENDGAME);
                    break;

                case Common.GameState.MainMenu:
                    UnLoadScene(GameScenes.GAME);
                    UnLoadScene(GameScenes.ENDGAME);
                    LoadScene(GameScenes.UI);
                    break;

                default:
                    break;
            }

        }

        private void LoadScene(GameScenes scene)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync((int) scene, LoadSceneMode.Additive);
            if (ao == null)
            {
                Debug.LogError("[GameManager] Error loading level." + scene);
                return;
            }
            _loadOperations.Add(ao);
            ao.completed += OnSceneLoadComplete;
        }

        private void UnLoadScene(GameScenes scene)
        {
            AsyncOperation ao = SceneManager.UnloadSceneAsync((int) scene);

            ao.completed += OnUnSceneLoadComplete;
        }


        private void OnSceneLoadComplete(AsyncOperation ao)
        {
            if (_loadOperations.Contains(ao))
            {
                _loadOperations.Remove(ao);

            }
            Debug.Log("Level load complete!");
        }
        private void OnUnSceneLoadComplete(AsyncOperation ao)
        {
            Debug.Log("Level unloaded complete!");
        }

    }

}

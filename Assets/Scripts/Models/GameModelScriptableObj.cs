
using UnityEngine;
using Utils;

namespace Models
{    /*  SUMMARY
     *  The GameModel is responsible of holding the game data that is required by multiple components.
     */
    [CreateAssetMenu(fileName = "GameModel", menuName = "ScriptableObjects/GameModel")]
    public class GameModelScriptableObj : ScriptableObject
    {
        // Game mode that was chosen in the main menu, e.g player vs computer.
        public Common.GameMode ChosenGameMode;
        // The state of the game, e.g main menu, turn started, etc.
        public Common.GameState CurrentGameState;
        // The end state of each game, e.g draw, player1 win, etc.
        public Common.EndRoundState CurrentEndState;

    }

}

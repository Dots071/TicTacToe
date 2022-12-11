using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    /*  SUMMARY
     *  The GridModel is responsible of holding grid data that is required by multiple components.
     */
    [CreateAssetMenu(fileName = "GridModel", menuName = "ScriptableObjects/GridModel")]

    public class GridModelScriptableObj : ScriptableObject
    {
        // The multiplier of rows and columns for creating the grid - i.e 3x3
        public int GridSize;
        // The turn index counting from the start of each game.
        public int TurnIndex;
        // The time between turn ended to started (next turn message display, etc.).
        public int TurnTransitionDelay = 1700;
        // Who's turn is it, player 1 is index 0.
        public int CurrentPlayerIndex;
        // Logs each new move that was made, resets on end game.
        public List<int[]> MovesLog = new List<int[]>();
    }
}

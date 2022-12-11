using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Holds all enums that are used on more than one script.
    /// </summary>
    public class Common
    {
        public enum TileMark
        {
            Empty = 0,
            X = 1,
            O = 2
        }

        public enum GameMode
        {
            TwoPlayers = 0,
            VsComputer = 1
        }

        public enum GameState
        {
            MainMenu = 0,
            GameStarting = 1,
            PreTurnTransition = 2,
            TurnStarted = 3,
            GameEnded = 4
        }

        public enum EndRoundState
        {
            None = 0,
            Player1Win = 1,
            Player2Win = 2,
            Draw = 3,
            TimesUp = 4
        }
    }
}


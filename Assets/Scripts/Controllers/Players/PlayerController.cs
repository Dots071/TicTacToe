using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Controllers.Players
{
    /// <summary>
    /// PlayerController - is one of the player types that are impelementing the ICanPlay interface.
    /// It gets the tile clicked data and passes the move to the GridManager.
    /// </summary>
    public class PlayerController : ICanPlay, IDisposable
    {
        private bool _isPlayersTurn = false;
        private bool _isTurnCancelled = false;

        // a cached callback to activate once we have a move to make (the Command Pattern).
        private Action<int, int> _turnResultCallback;

        public PlayerController()
        {
                GameEvents.Instance.TilePressed += OnTilePressed;
        }


        private void OnTilePressed(int row, int column)
        {
            if (_isPlayersTurn == false)
                return;

            _isPlayersTurn = false;

            if (_turnResultCallback != null && _isTurnCancelled == false)
            {
                // send CB to the GridManager and delete reference.
                _turnResultCallback(row, column);
                _turnResultCallback = null;
            }
        }

        // In the Player's case (unlike the computer's), there is no use for the movesLog as the check is done by the Tile. 
        // The CB is sent to the grid once the player has a valid result.
        public void PlayTurn(Action<int, int> turnPlayedCallback, List<int[]> movesLog)
        {
            _isPlayersTurn = true;
            _isTurnCancelled = false;

            //Cache callback
            _turnResultCallback = turnPlayedCallback;
        }

        public void CancelTurn()
        {
            _isTurnCancelled = true;
        }

        public void Dispose()
        {
            GameEvents.Instance.TilePressed -= OnTilePressed;
        }
    }
}

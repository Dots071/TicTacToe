using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Controllers.Players
{
    public class ComputerPlayer : ICanPlay
    {
        /// <summary>
        /// a controller for playing against the computer, returns a random valid move after a few seconds delay.
        /// </summary>
        private bool _isTurnCancelled = false;

        public async void PlayTurn(Action<int, int> tilePlayedCallback, List<int[]> movesLog)
        {
            _isTurnCancelled = false;
            int[] move = GetRandomMove();

            // When checking if move is valid, checkIndex is for preventing an endless loop.
            int checkIndex = 1;
            bool validMove = false;

            while (validMove == false)
            {
                if (checkIndex > 1000)
                    break;

                validMove = CheckIfValidMove(movesLog, move);

                // If random move is in the moves log history then find another tile.
                if (validMove == false)
                    move = GetRandomMove();

                checkIndex++;
            }

            await Task.Delay(3000);

            // return move result to gridManager.
            if(_isTurnCancelled == false)
                tilePlayedCallback(move[0], move[1]);
        }


        private int[] GetRandomMove()
        {
            int[] move = new int[2];

            // get random row
            move[0] = UnityEngine.Random.Range(0, 3);
            // get random column
            move[1] = UnityEngine.Random.Range(0, 3);

            return move;
        }

        private bool CheckIfValidMove(List<int[]> movesLog, int[] move)
        {
            for (int i = 0; i < movesLog.Count; i++)
            {
                if (movesLog[i][0] == move[0] && movesLog[i][1] == move[1])
                {
                    // Move was found in the movesLog.
                    return false;
                }
            }
            // The move hasn't been played yet and therefore valid.
            return true;
        }

        // this method is mandatory from the ICanPlay interface.
        public void CancelTurn()
        {
            _isTurnCancelled = true;
        }

    }
}


using Models;

namespace Controllers.GameGrid
{
    public class WinChecker
    {
        /// <summary>
        /// WinChecker is a service for checking if the round was ended with a win/lose.
        /// </summary>
        /// <param name="turnCount"> how many turns were playded. </param>
        /// <param name="selectedRow"> last move's row </param>
        /// <param name="selectedColumn"> last move's column </param>
        /// <param name="tiles"> tiles matrix reference. </param>
        /// <returns></returns>
        public bool CheckForWin(int turnCount, int selectedRow, int selectedColumn, TileModel[,] tiles)
        {
            var markChecked = tiles[selectedRow, selectedColumn].CurrentMark;
            var gridSize = tiles.GetLength(0);

            // Check col
            for (int i = 0; i < gridSize; i++)
            {
                if (tiles[selectedRow, i].CurrentMark != markChecked)
                    break;
                if (i == gridSize - 1)
                {
                    return true;
                }
            }

            // Check row
            for (int i = 0; i < gridSize; i++)
            {
                if (tiles[i, selectedColumn].CurrentMark != markChecked)
                    break;
                if (i == gridSize - 1)
                {
                    return true;
                }
            }

            // Check diagonal
            if (selectedRow == selectedColumn)
            {
                //we're on a diagonal
                for (int i = 0; i < gridSize; i++)
                {
                    if (tiles[i, i].CurrentMark != markChecked)
                        break;
                    if (i == gridSize - 1)
                    {
                        return true;
                    }
                }
            }

            // Check anti diagonal
            if (selectedRow + selectedColumn == gridSize - 1)
            {
                for (int i = 0; i < gridSize; i++)
                {
                    if (tiles[i, (gridSize - 1) - i].CurrentMark != markChecked)
                        break;
                    if (i == gridSize - 1)
                    {
                        return true;
                    }
                }
            }

            // No win
            return false;
        }
    }

}

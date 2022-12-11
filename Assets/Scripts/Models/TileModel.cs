using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Models
{
    /// <summary>
    /// TileModel is a model for holding public relevant data.
    /// </summary>
    public class TileModel
    {
        // the row and column the tile is located on the grid.
        public int RowIndex;
        public int ColumnIndex;

        public Common.TileMark CurrentMark;

        public TileModel(int row, int column, Common.TileMark initMark)
        {
            RowIndex = row;
            ColumnIndex = column;
            CurrentMark = initMark;
        }
    }

}

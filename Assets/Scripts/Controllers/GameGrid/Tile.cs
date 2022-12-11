using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Utils;
using Models;
using System;

namespace Controllers.GameGrid
{
    public class Tile : MonoBehaviour

    {
        /// <summary>
        /// The Tile class is responsible for getting and updating tile data.
        /// </summary>
        
        // inserted in the editor according to the tile location.
        [SerializeField] private int _rowIndex;
        [SerializeField] private int _columnIndex;
        
        // Reference the X & O images that are activated/deactivated to show mark.
        [SerializeField] private GameObject _xImageObject;
        [SerializeField] private GameObject _oImageObject;

        // set in the editor for click reference 
        [SerializeField] private Button tileButtonComponent;
        // holds all public tile data.
        public TileModel TileModel;

        // Parameters for preventing multiple clicks on one turn.
        private bool _tileClicked = false;
        private int _clickDelay = 500;

        private void Start()
        {
            tileButtonComponent.onClick.AddListener(SendPlayerClickResults);
            GameEvents.Instance.UpdateTileMark += OnUpdateTileMark;

            TileModel = new TileModel(_rowIndex, _columnIndex, Common.TileMark.Empty);
        }

        private void OnDisable()
        {
            tileButtonComponent.onClick.RemoveListener(SendPlayerClickResults);
            if(GameEvents.isInitialized)
                GameEvents.Instance.UpdateTileMark -= OnUpdateTileMark;

        }

        // called once move was made and was validated.
        private void OnUpdateTileMark(int row, int column, Common.TileMark mark)
        {
            if (TileModel.RowIndex == row && TileModel.ColumnIndex == column)
                UpdateTileMark(mark);
        }

        // on tile clicked will send to the player for processing.
        private async void SendPlayerClickResults()
        {
            Debug.Log("Tile " + TileModel.RowIndex + "," + TileModel.ColumnIndex + " was clicked!");
            if (TileModel.CurrentMark != Common.TileMark.Empty)
                return;

            if (_tileClicked == false)
            {
                _tileClicked = true;
                GameEvents.Instance.InvokeTilePressed(TileModel.RowIndex, TileModel.ColumnIndex);

                // Wait to prevent click abuse by the user.
                await Task.Delay(_clickDelay);
                _tileClicked = false;
            }
        }

        public void UpdateTileMark(Common.TileMark mark)
        {
            TileModel.CurrentMark = mark;

            switch (mark)
            {
                case Common.TileMark.Empty:
                    _xImageObject.SetActive(false);
                    _oImageObject.SetActive(false);
                    break;

                case Common.TileMark.X:
                    _xImageObject.SetActive(true);
                    break;

                case Common.TileMark.O:
                    _oImageObject.SetActive(true);
                    break;

                default:
                    break;
            }
        }

    }
}


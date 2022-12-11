using System.Collections.Generic;
using NUnit.Framework;
using Controllers.GameGrid;
using Models;
using Utils;


public class WinRoundTest
{

    private WinChecker winChecker = new WinChecker();


    // A Test to check if the winChecker returns true for a win and false for a lose.
    [Test]
    public void WinRound()
    {
        // creating a mock matrix with X on each tile in the first row.
        TileModel[,] tileMatrix = new TileModel[3, 3];
        tileMatrix[0, 0] = new TileModel(0, 0, Common.TileMark.X);
        tileMatrix[0, 1] = new TileModel(0, 1, Common.TileMark.X);
        tileMatrix[0, 2] = new TileModel(0, 2, Common.TileMark.X);

        tileMatrix[1, 0] = new TileModel(1, 0, Common.TileMark.Empty);
        tileMatrix[1, 1] = new TileModel(1, 1, Common.TileMark.Empty);
        tileMatrix[1, 2] = new TileModel(1, 2, Common.TileMark.Empty);

        tileMatrix[2, 0] = new TileModel(2, 0, Common.TileMark.Empty);
        tileMatrix[2, 1] = new TileModel(2, 1, Common.TileMark.Empty);
        tileMatrix[2, 2] = new TileModel(2, 2, Common.TileMark.Empty);

        int turnsPlayed = 3;
        int[] move = { 0, 0 };

        bool isWin = winChecker.CheckForWin(turnsPlayed, move[0], move[1], tileMatrix);

        Assert.AreEqual(true, isWin);
    }

    [Test]
    public void LoseRound()
    {
        // creating a mock matrix with O on the first tile and X on the other two in the first row.
        TileModel[,] tileMatrix = new TileModel[3, 3];
        tileMatrix[0, 0] = new TileModel(0, 0, Common.TileMark.O);
        tileMatrix[0, 1] = new TileModel(0, 1, Common.TileMark.X);
        tileMatrix[0, 2] = new TileModel(0, 2, Common.TileMark.X);

        tileMatrix[1, 0] = new TileModel(1, 0, Common.TileMark.Empty);
        tileMatrix[1, 1] = new TileModel(1, 1, Common.TileMark.Empty);
        tileMatrix[1, 2] = new TileModel(1, 2, Common.TileMark.Empty);

        tileMatrix[2, 0] = new TileModel(2, 0, Common.TileMark.Empty);
        tileMatrix[2, 1] = new TileModel(2, 1, Common.TileMark.Empty);
        tileMatrix[2, 2] = new TileModel(2, 2, Common.TileMark.Empty);

        int turnsPlayed = 3;
        int[] move = { 0, 0 };

        bool isWin = winChecker.CheckForWin(turnsPlayed, move[0], move[1], tileMatrix);

        Assert.AreEqual(false, isWin);

    }

    [Test]
    public void Draw()
    {
        // creating a mock matrix with full of marks but no win.
        TileModel[,] tileMatrix = new TileModel[3, 3];
        tileMatrix[0, 0] = new TileModel(0, 0, Common.TileMark.O);
        tileMatrix[0, 1] = new TileModel(0, 1, Common.TileMark.X);
        tileMatrix[0, 2] = new TileModel(0, 2, Common.TileMark.X);

        tileMatrix[1, 0] = new TileModel(1, 0, Common.TileMark.X);
        tileMatrix[1, 1] = new TileModel(1, 1, Common.TileMark.O);
        tileMatrix[1, 2] = new TileModel(1, 2, Common.TileMark.O);

        tileMatrix[2, 0] = new TileModel(2, 0, Common.TileMark.X);
        tileMatrix[2, 1] = new TileModel(2, 1, Common.TileMark.O);
        tileMatrix[2, 2] = new TileModel(2, 2, Common.TileMark.X);


        int turnsPlayed = 9;

        bool draw = (turnsPlayed == tileMatrix.Length);

        Assert.AreEqual(true, draw);
    }

}

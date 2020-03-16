using System;

namespace ConnectFour.Logic
{

    public class Board
    {

        public const byte columns = 7; // MUST be greater or equal 4!
        public const byte rows = 6; // MUST be greater or equal 4!

        /// <summary>
        /// [Column, Row]
        /// </summary>
        private readonly byte[,] GameBoard = new byte[columns,rows];

        internal int Player = 0;

        public void AddStone(byte column)
        {
            if (column > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            for (int row = 0; row < rows; row++)
            {
                var cell = GameBoard[column, row];

                if (cell == 0)
                {
                    Player = (Player + 1) % 2;
                    GameBoard[column, row] = (byte)(Player + 1);
                    return;
                }
            }

            throw new InvalidOperationException("Column is full");
        }

        public enum GameEnd
        {
            Tie,
            PlayerWins
        }

        public (bool,GameEnd?) IsGameOver()
        {
            // first: won?
            // if not: is board full
            // not the other way around!

            if (IsBoardFull())
            {
                return (true, GameEnd.Tie);
            }

            // default: game is not over
            return (false, null);
        }

        public bool IsBoardFull()
        {
            for (byte column = 0; column < columns; column++)
            {
                if (GameBoard[column,rows-1] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public enum GameWon
        {
            Horizontal,
            Vertical,
            Diagonal
        }

        public (bool, GameWon?) IsGameWon()
        {
            if (HorizontalWin()) return (true, GameWon.Horizontal);
            
            return (false, null);
        }


        public bool HorizontalWin()
        {
            for (byte row = 0; row < rows; row++)
            {
                // check if middle row is assigned 
                // if not, then horizontal win is not possible
                // only when columns are 7 or less
                // whey check? performance
                if (columns <= 7 && GameBoard[((byte)(columns/2)),row] == 0)
                {
                    break;
                }

                byte currentPlayerToCheck = 0;
                byte stonesInARow = 0;

                // adds a stones to stonesInARow and returns true if a player has won (horizontally) eg. four in a horizontal row
                bool addStoneInARowAndCheckForWin() => ++stonesInARow == 4;

                for (byte currentColumnToCheck = 0; currentColumnToCheck < columns; currentColumnToCheck++)
                {

                    // if there is no way, someone could have four in a horizontal row: continue with next row => break
                    if ((columns - currentPlayerToCheck) + stonesInARow < 4) break; 

                    if (GameBoard[currentColumnToCheck, row] == 0)
                    {
                        currentPlayerToCheck = 0;
                        stonesInARow = 0;
                    }
                    else 
                    {
                        // if same player: add stone
                        if (currentPlayerToCheck != 0 && GameBoard[currentColumnToCheck, row] == currentPlayerToCheck)
                        {
                            if (addStoneInARowAndCheckForWin()) return true;
                        } 
                        else
                        {
                            // if not the same player: stonesInARow should be 1 and currentPlayer should be changed
                            currentPlayerToCheck = GameBoard[currentPlayerToCheck, row];
                            stonesInARow = 1;
                        }

                    }
                }
            }
            return false;
        }


    }
}

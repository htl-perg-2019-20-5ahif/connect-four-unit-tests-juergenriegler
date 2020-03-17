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
            if (column > columns)
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
            if (IsGameWon().Item1)
            {
                return (true, GameEnd.PlayerWins);
            }

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
            if (VerticalWin()) return (true, GameWon.Vertical);
            if (DiagonalWin()) return (true, GameWon.Diagonal);
            
            return (false, null);
        }


        /*
         * The Idea is to interate through every column and shift down the left columns and rights columns accordingly
         * Then it can simply be checked for a horizontal win
         * finished!
         */
        public bool DiagonalWin()
        {
            for (byte column = 0; column < columns; column++)
            {
                var gameBoardCopy = GameBoard.Clone() as byte[,];

                // don't have to shift left ones if there are none
                if (column != 0)
                {
                    // cannot: byte columnsLeftShift = column-1 // int to byte conversion
                    byte columnsLeftNextToShift = column;
                    columnsLeftNextToShift -= 1;
                    byte columnsLeftToShiftBy = 1;
                    if (columnsLeftNextToShift >= 0)
                    {
                        // if columnsLeftNextToShift is 0 and -- is called, it goes to 255 => so check for 255 instead of is less then 0
                        for (; columnsLeftNextToShift != 255; columnsLeftNextToShift--)
                        {
                            for (byte row = 0; row < rows; row++)
                            {
                                // cannot shift no more
                                if (row + columnsLeftToShiftBy >= rows)
                                {
                                    break;
                                }
                                gameBoardCopy[columnsLeftNextToShift, row] = gameBoardCopy[columnsLeftNextToShift, row + columnsLeftToShiftBy];
                            }
                            columnsLeftToShiftBy += 1;
                        }
                    }
                }
                
                // don't have to shift right ones if there are none
                if (column != columns-1)
                {
                    // cannot: byte columnsRightShift = column+1 // int to byte conversion
                    byte columnsRightNextToShift = column;
                    columnsRightNextToShift += 1;
                    byte columnsRightToShiftBy = 1;

                    if (columnsRightNextToShift < columns)
                    {
                        for (; columnsRightNextToShift < columns; columnsRightNextToShift++)
                        {
                            for (byte row = 0; row < rows; row++)
                            {
                                // cannot shift no more
                                if (row + columnsRightToShiftBy >= rows)
                                {
                                    break;
                                }
                                gameBoardCopy[columnsRightNextToShift, row] = gameBoardCopy[columnsRightNextToShift, row + columnsRightToShiftBy];
                            }
                            columnsRightToShiftBy += 1;
                        }
                    }
                }

                if (HorizontalWin(gameBoardCopy)) return true;
                
            }
            return false;
        }


        public bool HorizontalWin(byte[,] board = null)
        {
            if (board == null) board = GameBoard;

            for (byte row = 0; row < rows; row++)
            {
                // check if middle column is assigned 
                // if not, then horizontal win is not possible
                // only when columns are 7 or less
                // whey check? performance
                if (columns <= 7 && board[(byte)(columns/2),row] == 0)
                {
                    continue;
                }

                byte currentPlayerToCheck = 0;
                byte stonesInARow = 0;

                // adds a stones to stonesInARow and returns true if a player has won (horizontally) eg. four in a horizontal row
                bool addStoneInARowAndCheckForWin() => ++stonesInARow == 4;

                for (byte currentColumnToCheck = 0; currentColumnToCheck < columns; currentColumnToCheck++)
                {

                    // if there is no way, someone could have four in a horizontal row: continue with next row => break
                    if ((columns - currentColumnToCheck) + stonesInARow < 4) break; 

                    if (board[currentColumnToCheck, row] == 0)
                    {
                        currentPlayerToCheck = 0;
                        stonesInARow = 0;
                    }
                    else 
                    {
                        // if same player: add stone
                        if (currentPlayerToCheck != 0 && board[currentColumnToCheck, row] == currentPlayerToCheck)
                        {
                            if (addStoneInARowAndCheckForWin()) return true;
                        } 
                        else
                        {
                            // if not the same player: stonesInARow should be 1 and currentPlayer should be changed
                            currentPlayerToCheck = board[currentColumnToCheck, row];
                            stonesInARow = 1;
                        }

                    }
                }
            }
            return false;
        }

        public bool VerticalWin(byte[,] board = null)
        {
            if (board == null) board = GameBoard;

            for (byte column = 0; column < columns; column++)
            {
                // check if middle row is assigned 
                // if not, then horizontal win is not possible
                // only when rows are 7 or less
                // whey check? performance
                if (rows <= 7 && board[column,(byte)(rows/2)] == 0)
                {
                    continue;
                }

                byte currentPlayerToCheck = 0;
                // in a row does not refer to the grids row
                byte stonesInARow = 0;

                 // adds a stones to stonesInARow and returns true if a player has won (vertically) eg. four in a vertical row
                bool addStoneInARowAndCheckForWin() => ++stonesInARow == 4;

                for (byte currentRowToCheck = 0; currentRowToCheck < rows; currentRowToCheck++)
                {

                    // if there is no way, someone could have four in a vertical line: continue with next row => break
                    if ((rows - currentRowToCheck) + stonesInARow < 4) break; 

                    if (board[column, currentRowToCheck] == 0)
                    {
                        currentPlayerToCheck = 0;
                        stonesInARow = 0;
                    }
                    else 
                    {
                        // if same player: add stone
                        if (currentPlayerToCheck != 0 && board[column, currentRowToCheck] == currentPlayerToCheck)
                        {
                            if (addStoneInARowAndCheckForWin()) return true;
                        } 
                        else
                        {
                            // if not the same player: stonesInARow should be 1 and currentPlayer should be changed
                            currentPlayerToCheck = board[column, currentRowToCheck];
                            stonesInARow = 1;
                        }

                    }
                }
            }
            return false;
        }

        public string GetGameBoardTextualGraphic()
        {
            var gameboardString = "";
            
            for (var row = rows-1; row >= 0; row--)
            {
                for (var column = 0; column < columns; column++)
                {
                    gameboardString += GameBoard[column, row] + " ";
                }
                gameboardString += "\n";
            }

            return gameboardString;
        }

    }
}

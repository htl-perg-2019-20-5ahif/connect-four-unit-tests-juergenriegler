using System;

namespace ConnectFour.Logic
{
    public class Board
    {
        /// <summary>
        /// [Column, Row]
        /// </summary>
        private readonly byte[,] GameBoard = new byte[7,6];

        internal int Player = 0;

        public void AddStone(byte column)
        {
            if (column > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            for (int row = 0; row < 6; row++)
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

        public bool IsGameOver()
        {
            // first: won?
            // if not: is board full
            // not the other way around!
            
            return false;
        }

        public bool IsBoardFull()
        {
            for (int column = 0; column < 7; column++)
            {
                if (GameBoard[column,5] == 0)
                {
                    return false;
                }
            }
            return true;
        }

    }
}

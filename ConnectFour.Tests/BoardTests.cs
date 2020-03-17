using ConnectFour.Logic;
using System;
using Xunit;

namespace ConnectFour.Tests
{
    public class BoardTests
    {
        [Fact]
        public void AddWithInvalidColumnIndex()
        {
            var b = new Board();

            Assert.Throws<ArgumentOutOfRangeException>(() => b.AddStone(7));
        }

        [Fact]
        public void PlayerChangesWhenAddingStone()
        {
            var b = new Board();

            var oldPlayer = b.Player;
            b.AddStone(0);

            // Verify that player has changed
            Assert.NotEqual(oldPlayer, b.Player);
        }

        [Fact]
        public void AddingTooManyStonesToARow()
        {
            var b = new Board();

            for(var i=0; i<6; i++)
            {
                b.AddStone(0);
            }

            var oldPlayer = b.Player;
            Assert.Throws<InvalidOperationException>(() => b.AddStone(0));
            Assert.Equal(oldPlayer, b.Player);
        }


        [Fact]
        public void BoardFull()
        {
            var board = new Board();

            for (byte column = 0; column < 7; column++)
            {
                for (byte row = 0; row < 6; row++)
                {
                    // with this line: one last stone is left out for the board to not be full
                    if (column == 6 && row == 5) break;

                    board.AddStone(column);
                }
            }
            Assert.False(board.IsBoardFull());

            // the last stone is not added
            board.AddStone(6);
            Assert.True(board.IsBoardFull());

        }


        [Fact]
        public void HorizontalWin()
        {
            var board = new Board();
            
            // no win
            for (byte column = 0; column < 3; column++)
            {
                // execute twice so that a horizontal line can be built, otherwise there would be player 1, 2, 1, 2, ...
                board.AddStone(column);
                board.AddStone(column);
            }
            Assert.False(board.HorizontalWin());

            // now: win
            board.AddStone(3);
            Assert.True(board.HorizontalWin());
        }

        [Fact]
        public void VerticalWin()
        {
            var board = new Board();

            // no win
            for (byte row = 0; row < 3; row++)
            {
                // execute twice so that a vertical line can be built, otherwise there would be player 1, 2, 1, 2, ...
                board.AddStone(0);
                board.AddStone(1);
            }
            Assert.False(board.VerticalWin());

            // now: win
            board.AddStone(0);
            Assert.True(board.VerticalWin());
        }

        [Fact]
        public void TestDiagonal()
        {
            var board = new Board();
            board.AddStone(1); // player 1 1x
            board.AddStone(2); // player 2
            board.AddStone(2); // player 1 2x
            board.AddStone(3); // player 2
            board.AddStone(4); // player 1
            board.AddStone(3); // player 2
            board.AddStone(3); // player 1 3x
            board.AddStone(4); // player 2
            board.AddStone(4); // player 1
            board.AddStone(5); // player 2

            // no win
            Assert.False(board.DiagonalWin());

            // now: win
            board.AddStone(4); // player 1
            Assert.True(board.DiagonalWin());
        }

        [Fact]
        public void TestWon()
        {
            var board = new Board();

            // no win
            for (byte row = 0; row < 3; row++)
            {
                // execute twice so that a vertical line can be built, otherwise there would be player 1, 2, 1, 2, ...
                board.AddStone(0);
                board.AddStone(1);
            }
            Assert.False(board.IsGameWon().Item1);

            // now: win
            board.AddStone(0);
            Assert.True(board.IsGameWon().Item1);
            Assert.True(board.IsGameWon().Item2 == Board.GameWon.Vertical);
        }

    }
}

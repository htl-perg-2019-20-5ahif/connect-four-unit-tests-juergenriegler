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

    }
}

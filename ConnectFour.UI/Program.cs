using ConnectFour.Logic;
using System;

namespace ConnectFour.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            StartGame();
        }
        
        private static void StartGame()
        {
            var board = new Board();
            do
            {
                Console.WriteLine(board.GetGameBoardTextualGraphic());


                // read from console and put stone
                var continueRead = false;
                do
                {
                    
                    try
                    {
                        Console.WriteLine("\nSelect Column:");
                        var numberRead = int.Parse(Console.ReadLine().Trim());
                        byte column = (byte)numberRead;
                        board.AddStone(column);
                        continueRead = false;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("Please input a number between 0 and " + (byte)(Board.columns-(byte)1));
                        continueRead = true;
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("This column is full. Please choose another one.");
                        continueRead = true;
                    }
                } while (continueRead);

            } while (board.IsGameOver().Item1 == false);
            Console.WriteLine(board.GetGameBoardTextualGraphic());
            var gameOverReason = board.IsGameOver().Item2;
            if (gameOverReason == Board.GameEnd.Tie) Console.WriteLine("Tie!");
            if (gameOverReason == Board.GameEnd.PlayerWins) Console.WriteLine("You won!");
        }

    }
}

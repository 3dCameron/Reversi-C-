using System;

namespace Reversi {

    public class Game
    {
        Board board = new Board();
        bool turn = false;
        bool active = false;

        //////////////
        // GAMEPLAY //
        //////////////

        public void Start() {
            active = true;

            while (active) {
                active = (TakeTurn() && board.CheckForLegalMoves(turn));
            }

            End();
        }

        // Prompt the user for input, displaying an error message if the previous input was invalid.
        private string RequestInput(bool error) {
            Console.Clear();
            Console.WriteLine(board.GetBoardAsString());
            if (error) {
                Console.WriteLine("That was not a valid move. Please try again.");
            }
            Console.WriteLine($"{(turn ? "White" : "Black")}, type \"EXIT\" to end the game, or enter the cell in which to place your next piece (for example, \"I9\"):");
            return Console.ReadLine();

        }

        // Get user input and try to complete a move.
        private bool TakeTurn() {
            bool success = true;

            string input;

            do {
                input = RequestInput(!success);

                if (input.ToUpper().Equals("EXIT")) {
                    return false;
                }

                success = MoveFromInput(input);
            } while (!success);
            
            NextTurn();

            return true;
        }

        private bool MoveFromInput(string input) {
            int row = 0;
            int col = 0;

            // Make sure the input meets the length requirement before parsing
            if (input.Length == 2) {
                row = (int)(Char.ToUpper(input[0])) - 65;
                col = (int)Char.GetNumericValue(input[1]) - 1;

                if (row >= 0 && row < 8 && col >= 0 && col < 8) {
                    return (board.GetCellAt(row, col).PlacePiece(turn));
                }
            }

            return false;
        }

        private void NextTurn() {
            turn = !turn;
        }

        // Tally the score and declare a winner
        public void End() {
            // This has likely already been made false, but let's be safe
            active = false;

            int[] scores = board.GetScores();

            String message = "Black wins!";

            if (scores[0] == scores[1]) {
                message = "The game is a draw.";
            } else if (scores[0] < scores[1]) {
                message = "White wins!";
            }

            Console.Clear();

            Console.WriteLine(
                board.GetBoardAsString() +
                "\nGame Over!\n" +
                "The final score is:\n" +
                "Black: " + scores[0] + "\tWhite: " + scores[1] + "\n" +
                message
            );
            
        } 

    }
}
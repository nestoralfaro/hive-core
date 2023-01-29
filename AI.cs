namespace GameCore
{
    public class AI : Player
    {
        public AI(Color color) : base(color) {}

        private const int _MAX_DEPTH = 5;

        public void MakeMove(Board board)
        {
            /**
            // Place it on the board
            Move myMove = new Move("bS1NEbQ1");
            Piece myPiece = new Piece("bS1", (0, 0));
            board.PlacePiece(this, myMove, myPiece, (0, 0));

            // Or Move a piece 
            Move myMove = new Move("bS1NEbQ1");
            (int, int) from = (0, 0);
            (int, int) to = (1, 1);
            board.MovePiece(board.Pieces[from].Peek(), to);
            **/
        }

        public (int, Move) alpha_beta(Board board, int alpha, int beta, int depth)
        {

            if (depth > _MAX_DEPTH || board.IsAQueenSurrounded())
            {
                // Calculate score with heuristic
                // Return score and currentMove
            }

            // if AI turn
                int maxValue = -1000000;
                // For every movable/placeable piece for AI color

                // first movable




                // then placeable
                var placeables = GetPlacingSpots(board.Pieces, board._color_pieces, board.IsAQueenSurrounded());
                // for every available move 
                foreach (var placing in placeables)
                {
                    // for current piece
                    foreach (string myPiece in Pieces)
                    {
                        // Make move 
                        board.PlacePiece(this, new Piece(myPiece, placing), placing);

                        // switch whose turn, depth + 1
                        // (min, move) = alpha_beta(alpha, beta, depth)

                        // if (min > maxValue)
                            // maxValue = min
                            // myMove = move

                        // set game state back to wehat is was before making move

                        // If alpha catches up to beta, kill kids
                        // if (maxValue >= beta)
                            // return (maxValue, myMove)

                        // new value is greater than alpha? update alpha
                        // if (maxValue > alpha)
                            // alpha = maxValue
                    }
                }

            // if Player turn
                // int minValue = 1000000
                // For every movable/placeable piece for PLAYER color
                    // for every available move for current piece
                        // Make move 
                        // switch whose turn, depth + 1
                        // (max, move) = alpha_beta(alpha, beta, depth)

                        // if (max < minValue)
                            // maxValue = min
                            // myMove = move

                        // set game state back to what is was before making move

                        // If alpha catches up to beta, kill kids
                        // if (minValue <= beta)
                            // return (minValue, myMove)

                        // new value is greater than alpha? update alpha
                        // if (minValue < alpha)
                            // beta = minValue

                // Return (minValue, myMove)
            return (-1, new Move("somemove"));
        }
    }
}



// namespace GameCore
// {
//     public class AI : Player
//     {
//         int maxDepth = 5;

//         public (int, Move) alpha_beta(alpha, beta, depth)
//         {
//             Move mymove = Nothing;

//             // initialize scratch game if needed

//             if (depth > maxDepth || IsGameOver())
//             {
//                 // Calculate score with heuristic
//                 // Return score and currentMove
//             }

//             // if AI turn
//                 // int maxValue = -1000000
//                 // For every movable/placeable piece for AI color
//                     // for every available move for current piece
//                         // Make move 
//                         // switch whose turn, depth + 1
//                         // (min, move) = alpha_beta(alpha, beta, depth)

//                         // if (min > maxValue)
//                             // maxValue = min
//                             // myMove = move

//                         // set game state back to wehat is was before making move

//                         // If alpha catches up to beta, kill kids
//                         // if (maxValue >= beta)
//                             // return (maxValue, myMove)

//                         // new value is greater than alpha? update alpha
//                         // if (maxValue > alpha)
//                             // alpha = maxValue

//                 // Return (maxValue, myMove)

//             // if Player turn
//                 // int minValue = 1000000
//                 // For every movable/placeable piece for PLAYER color
//                     // for every available move for current piece
//                         // Make move 
//                         // switch whose turn, depth + 1
//                         // (max, move) = alpha_beta(alpha, beta, depth)

//                         // if (max < minValue)
//                             // maxValue = min
//                             // myMove = move

//                         // set game state back to what is was before making move

//                         // If alpha catches up to beta, kill kids
//                         // if (minValue <= beta)
//                             // return (minValue, myMove)

//                         // new value is greater than alpha? update alpha
//                         // if (minValue < alpha)
//                             // beta = minValue

//                 // Return (minValue, myMove)


                        

//         }
//     }
// }
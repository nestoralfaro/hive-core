namespace GameCore
{
    public class AI : Player
    {
        public AI(Color color) : base(color) {}

        private const int _MAX_DEPTH = 5;

        public override bool MakeMove(Board board)
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

            // if everything went well
            return true;
        }

        public (int, Move?) alpha_beta(Board board, int alpha, int beta, int depth)
        {
            Color whoseTurn = this.Color;
            Move? myMove = null;

            if (depth > _MAX_DEPTH || board.IsAQueenSurrounded())
            {
                // Calculate score with heuristic
                // Return score and currentMove
            }

            // if AI turn
            if (whoseTurn == this.Color)
            {
                int maxValue = -1000000;
                // For every movable/placeable piece for AI color

                // First alternative: run each in parallel?

                // first movable
                // for current piece
                foreach(var piece in board.Pieces)
                {
                    // for every available move 
                    foreach(var moving in piece.Value.Peek().GetMovingSpots(ref board))
                    {
                        Piece oldPieceState = piece.Value.Peek();

                        // Make move 
                        board.MovePiece(oldPieceState, moving);

                        // switch whose turn
                        whoseTurn = whoseTurn == Color.White ? Color.Black : Color.White;

                        ++depth;

                        (int min, Move? move) = alpha_beta(board, alpha, beta, depth);

                        if (min > maxValue)
                        {
                            maxValue = min;
                            myMove = move;
                        }

                        // set game state back to what is was before making move
                        board.MovePiece(oldPieceState, oldPieceState.Point);

                        // If alpha catches up to beta, kill kids
                        if (maxValue >= beta)
                            return (maxValue, myMove);

                        // new value is greater than alpha? update alpha
                        if (maxValue > alpha)
                            alpha = maxValue;
                    }
                }

                // then placeable
                var placeables = GetPlacingSpots(board.Pieces, board._color_pieces, board.IsAQueenSurrounded());
                // for current piece
                foreach (string myPiece in Pieces)
                {
                    // for every available move 
                    foreach (var placing in placeables)
                    {
                        Piece piece = new(myPiece, placing);

                        // Make move 
                        board.PlacePiece(this, piece, placing);

                        // switch whose turn
                        whoseTurn = whoseTurn == Color.White ? Color.Black : Color.White;

                        ++depth;

                        (int min, Move? move) = alpha_beta(board, alpha, beta, depth);

                        if (min > maxValue)
                        {
                            maxValue = min;
                            myMove = move;
                        }

                        // set game state back to what is was before making move
                        board.UndoPlacing(this, piece);

                        // If alpha catches up to beta, kill kids
                        if (maxValue >= beta)
                            return (maxValue, myMove);

                        // new value is greater than alpha? update alpha
                        if (maxValue > alpha)
                            alpha = maxValue;
                    }
                }
                return (maxValue, myMove);

                // Second alternative: run all at the same time but they might have to be labeled–i.e., moving or placing
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

            // Not relevant. Just so the compiler is happy
            return (-1, new Move("somemove"));
        }
    }
}



// Original pseudocode
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
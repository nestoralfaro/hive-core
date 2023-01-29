namespace GameCore
{
    public class AI : Player
    {
        private const int _MAX_DEPTH = 5;

        public AI(Color color) : base(color) {}

        public override bool MakeMove(Board board, Player player)
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

            (int maxVal, Move? move) res = alpha_beta(board, player, this.Color);

            // if everything went well
            return true;
        }

        public (int, Move?) alpha_beta(Board board, Player player, Color whoseTurn, int alpha = 0, int beta = 0, int depth = 1)
        {
            Move? myMove = null;

            if (depth > _MAX_DEPTH || board.IsAQueenSurrounded())
            {
                // Calculate score with heuristic
                // Return score and currentMove
            }

            int maxValue = -1000000;

            // For every movable/placeable piece for AI color

            // First alternative: run each in parallel?
            // Second alternative: run all at the same time but they might have to be labeled–i.e., moving or placing

            // first movables
            // for current piece
            foreach(var piece in board.GetPiecesByColor(whoseTurn))
            {
                // for every available move 
                foreach(var moving in piece.GetMovingSpots(ref board))
                {
                    Piece oldPieceState = piece;
                    // Make move 
                    board.MovePiece(oldPieceState, moving);
                    ++depth;
                                                                    // switch whose turn
                    (int min, Move? move) = alpha_beta(board, player, whoseTurn == Color.White ? Color.Black : Color.White, alpha, beta, depth);

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

            // then placeables
            var placeables = whoseTurn == this.Color
            // Get AI's placing spots
            ? GetPlacingSpots(board.Pieces, board._color_pieces, board.IsAQueenSurrounded())
            // Get player's placing spots
            : player.GetPlacingSpots(board.Pieces, board._color_pieces, board.IsAQueenSurrounded());

            // for current piece for the AI or Player (whoever's turn it is)
            foreach (string myPiece in whoseTurn == this.Color ? Pieces : player.Pieces)
            {
                // for every available move 
                foreach (var placing in placeables)
                {
                    Piece piece = new(myPiece, placing);

                    // Make move 
                    board.PlacePiece(this, piece, placing);

                    ++depth;

                                                                    // switch whose turn
                    (int min, Move? move) = alpha_beta(board, player, whoseTurn == Color.White ? Color.Black : Color.White, alpha, beta, depth);

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
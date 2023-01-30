namespace GameCore
{
    public class AI : Player
    {
        private const int _MAX_DEPTH = 5;

        public AI(Color color) : base(color) {}

        public override bool MakeMove(ref Board board, ref Player opponent)
        {
            Board playgroundBoard = new();
            playgroundBoard.ReplaceWithState(board);

            Player playgroundOpponent = new(opponent.Color);
            playgroundOpponent.ReplaceWithState(opponent);

            Player playgroundAI = new (this.Color);
            playgroundAI.ReplaceWithState(this);

            (int maxVal, AIAction? move) = alpha_beta(ref playgroundBoard, ref playgroundAI, ref playgroundOpponent, this.Color);

            if (move != null)
                board.AIMove(this, move.Piece, move.To);

            // if everything went well
            return true;
        }

        public (int, AIAction?) alpha_beta(ref Board board, ref Player AI, ref Player opponent, Color whoseTurn, int alpha = int.MinValue, int beta = int.MaxValue, int depth = 1)
        {
            AIAction? myMove = null;

            if (depth > _MAX_DEPTH || board.IsAQueenSurrounded())
            {
                // Calculate score with heuristic
                // Return score and currentMove
                return (1, myMove);
            }

            // If AI turn
            if (whoseTurn == AI.Color)
            {
                int maxValue = -1000000;
                List<Piece> aiPieces = new();

                // my pieces from my hand
                aiPieces.AddRange(AI.Pieces);

                // my pieces from the board
                aiPieces.AddRange(board.GetPiecesByColor(AI.Color));

                // For every AI piece
                foreach (var piece in aiPieces)
                {
                    List<(int, int)> availableMoves = new();

                    // if this piece is on the board
                    if (piece != null && board.IsOnBoard(piece.ToString()))
                    {
                        // what are my moving spots?
                        availableMoves.AddRange(piece.GetMovingSpots(ref board));
                    }

                    // if AI has this piece
                    if (piece != null && AI.Pieces.Contains(piece))
                    {
                        // what are my placing spots?
                        availableMoves.AddRange(GetPlacingSpots(ref board));
                    }
                    if (availableMoves.Count > 0)
                    {
                        // for every available move for current piece
                        foreach (var to in availableMoves)
                        {
                            if (piece != null)
                            {
                                // make move
                                board.AIMove(AI, piece, to);
                                                                                                        // switch whose turn
                                (int min, AIAction? move) = alpha_beta(ref board, ref AI, ref opponent, whoseTurn == Color.White ? Color.Black : Color.White, alpha, beta, depth + 1);

                                if (min > maxValue)
                                {
                                    maxValue = min;
                                    myMove = new AIAction(piece, to);
                                }

                                // set game state back to what is was before making move
                                board.Undo();

                                // If alpha catches up to beta, kill kids
                                if (maxValue >= beta)
                                    return (maxValue, myMove);

                                // new value is greater than alpha? update alpha
                                if (maxValue > alpha)
                                    alpha = maxValue;
                            }
                        }
                    }

                }
                return (maxValue, myMove);
            }

            // If Opponent turn
            // if (whoseTurn != this.Color)
            else
            {
                int minValue = 1000000;
                List<Piece> opponentPieces = new();

                // opponent's pieces from my hand
                opponentPieces.AddRange(opponent.Pieces);
                // opponent's pieces from the board
                opponentPieces.AddRange(board.GetPiecesByColor(opponent.Color));

                // For every OPPONENT's piece
                foreach (var piece in opponentPieces)
                {
                    List<(int, int)> availableMoves = new();

                    // if this piece is on the board
                    if (board.IsOnBoard(piece.ToString()))
                    {
                        // what are my opponent's moving spots?
                        availableMoves.AddRange(piece.GetMovingSpots(ref board));
                    }

                    // if my opponent has this piece
                    if (opponent.Pieces.Contains(piece))
                    {
                        // what are my opponent's placing spots?
                        availableMoves.AddRange(GetPlacingSpots(ref board));
                    }

                    // for every available move for current piece
                    foreach (var to in availableMoves)
                    {
                        // make move
                        board.AIMove(opponent, piece, to);
                                                                                                // switch whose turn
                        (int max, AIAction? move) = alpha_beta(ref board, ref AI, ref opponent, whoseTurn == Color.White ? Color.Black : Color.White, alpha, beta, depth + 1);

                        if (max < minValue)
                        {
                            minValue = max;
                            myMove = new AIAction(piece, to);
                        }

                        // set game state back to what is was before making move
                        board.Undo();

                        // If alpha catches up to beta, kill kids
                        if (minValue <= beta)
                            return (minValue, myMove);

                        // new value is greater than alpha? update alpha
                        if (minValue < alpha)
                            alpha = minValue;
                    }
                }
                return (minValue, myMove);
            }
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
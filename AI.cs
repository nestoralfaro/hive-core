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
                board.AIMove(this, move.Piece, move.To, move.IsMoving);

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
                return (new Random().Next(1, 50), myMove);
            }

            // If AI turn
            if (whoseTurn == AI.Color)
            {
                int maxValue = -1000000;
                List<Piece> aiPieces = new();

                // AI's pieces on the board
                aiPieces.AddRange(board.GetPiecesByColor(AI.Color));
                // AI's pieces on hand
                aiPieces.AddRange(AI.Pieces);

                // For every AI piece
                foreach (var piece in aiPieces)
                {
                    Dictionary<(int, int), bool> availableMoves = new();

                    // if this piece is on the board
                    if (board.IsOnBoard(piece.ToString()) && !AI.Pieces.Contains(piece))
                    {

                        // get its moving spots
                        foreach (var move in piece.GetMovingSpots(ref board))
                        {
                            availableMoves.Add(move, true);
                        }
                    }

                    // if AI has this piece
                    if (AI.Pieces.Contains(piece) && !board.IsOnBoard(piece.ToString()))
                    {
                        // get AI's placing spots
                        // availableMoves.AddRange(GetPlacingSpots(ref board));
                        foreach (var move in GetPlacingSpots(ref board))
                        {
                            availableMoves.Add(move, false);
                        }
                    }

                    // for every available move for current piece
                    foreach (var to in availableMoves)
                    {
                        // make move
                        board.AIMove(AI, piece, to.Key, to.Value);
                                                                                                // switch whose turn
                        (int min, AIAction? move) = alpha_beta(ref board, ref AI, ref opponent, whoseTurn == Color.White ? Color.Black : Color.White, alpha, beta, depth + 1);

                        if (min > maxValue)
                        {
                            maxValue = min;
                            myMove = new AIAction(piece, to.Key, to.Value);
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
                return (maxValue, myMove);
            }

            // If Opponent turn
            // if (whoseTurn != this.Color)
            else
            {
                int minValue = 1000000;
                List<Piece> opponentPieces = new();

                // opponent's pieces on the board
                opponentPieces.AddRange(board.GetPiecesByColor(opponent.Color));
                // opponent's pieces on hand
                opponentPieces.AddRange(opponent.Pieces);

                // For every OPPONENT's piece
                foreach (var piece in opponentPieces)
                {
                    // List<(int, int)> availableMoves = new();
                    Dictionary<(int, int), bool> availableMoves = new();

                    if (piece.ToString().Equals("bQ1"))
                    {
                        Console.WriteLine("breakpoing!");
                    }

                    // // if this piece is on the board
                    if (board.IsOnBoard(piece.ToString()) && !opponent.Pieces.Contains(piece))
                    {
                        // get its moving spots
                        // availableMoves.AddRange(piece.GetMovingSpots(ref board));
                        foreach (var move in piece.GetMovingSpots(ref board))
                        {
                            availableMoves.Add(move, true);
                        }
                    }

                    // // if opponent has this piece
                    if (opponent.Pieces.Contains(piece) && !board.IsOnBoard(piece.ToString()))
                    {
                        // get opponent's placing spots
                        // availableMoves.AddRange(opponent.GetPlacingSpots(ref board));
                        foreach (var move in opponent.GetPlacingSpots(ref board))
                        {
                            availableMoves.Add(move, false);
                        }
                    }

                    // for every available move for current piece
                    foreach (var to in availableMoves)
                    {
                        // make move
                        board.AIMove(opponent, piece, to.Key, to.Value);
                                                                                                // switch whose turn
                        (int max, AIAction? move) = alpha_beta(ref board, ref AI, ref opponent, whoseTurn == Color.White ? Color.Black : Color.White, alpha, beta, depth + 1);

                        if (max < minValue)
                        {
                            minValue = max;
                            myMove = new AIAction(piece, to.Key, to.Value);
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
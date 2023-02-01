// namespace HiveCore
// {
//     public class AI : Player
//     {
//         private const int _MAX_DEPTH = 5;

//         public AI(Color color) : base(color) {}
//         public override bool MakeMove(ref Board board)
//         {
//             Board dummyBoard = board.Clone();
//             Player dummyAI = this.Clone();
//             (int maxVal, AIAction? move) = alpha_beta(ref dummyBoard, ref dummyAI, this.Color);
//             if (move != null)
//                 board.AIMove(this, move.Action, move.Piece, move.To);
//             // if everything went well
//             return true;
//         }

//         public (int, AIAction?) alpha_beta(ref Board board, ref Player AI, Color whoseTurn, int alpha = int.MinValue, int beta = int.MaxValue, int depth = 1)
//         {
//             AIAction? myMove = null;

//             if (depth > _MAX_DEPTH || board.IsAQueenSurrounded())
//             {
//                 // Calculate score with heuristic
//                 // Return score and currentMove
//                 return (new Random().Next(1, 50), myMove);
//             }

//             // If AI turn
//             if (whoseTurn == AI.Color)
//             {
//                 int maxValue = -1000000;
//                 List<Piece> aiPieces = new();

//                 // AI's pieces on the board
//                 aiPieces.AddRange(board.GetClonePiecesByColor(AI.Color));
//                 // AI's pieces on hand
//                 aiPieces.AddRange(AI.Pieces.ConvertAll(piece => piece.Clone()));

//                 // For every AI piece
//                 foreach (var piece in aiPieces)
//                 {
//                     HashSet<(ActionKind action, (int, int) to)> availableMoves = new();

//                     // if this piece is on the board
//                     if (board.IsOnBoard(piece.ToString()) && !AI.Pieces.Contains(piece))
//                     {
//                         // get its moving spots
//                         foreach (var to in piece.GetMovingSpots(ref board))
//                         {
//                             availableMoves.Add((ActionKind.Moving, to));
//                         }
//                     }

//                     // if AI has pieces to play
//                     if (AI.Pieces.Count > 0)
//                     {
//                         // get AI's placing spots
//                         // availableMoves.AddRange(GetPlacingSpots(ref board));
//                         foreach (var to in GetPlacingSpots(ref board))
//                         {
//                             availableMoves.Add((ActionKind.Placing, to));
//                         }
//                     }

//                     // for every available move for current piece
//                     foreach (var availableMove in availableMoves)
//                     {
//                         Board temp = board.Clone();

//                         // make move
//                         if (availableMove.action == ActionKind.Moving && board.IsOnBoard(piece.ToString()))
//                         {
//                             board.MovePiece(piece, availableMove.to);
//                         }
//                         else if (availableMove.action == ActionKind.Placing && AI.Pieces.Any(p => p.ToString().Equals(piece)))
//                         {
//                             board.PlacePiece(AI, piece, availableMove.to);
//                         }

//                                                                                                 // switch whose turn
//                         (int min, AIAction? move) = alpha_beta(ref board, ref AI, ref opponent, whoseTurn == Color.White ? Color.Black : Color.White, alpha, beta, depth + 1);

//                         if (min > maxValue)
//                         {
//                             maxValue = min;
//                             myMove = new AIAction(piece, availableMove);
//                         }

//                         // set game state back to what is was before making move
//                         board = temp;

//                         // If alpha catches up to beta, kill kids
//                         if (maxValue >= beta)
//                             return (maxValue, myMove);

//                         // new value is greater than alpha? update alpha
//                         if (maxValue > alpha)
//                             alpha = maxValue;
//                     }
//                 }
//                 return (maxValue, myMove);
//             }

//             // If Opponent turn
//             // if (whoseTurn != this.Color)
//             else
//             {
//                 int minValue = 1000000;
//                 List<Piece> opponentPieces = new();

//                 // opponent's pieces on the board
//                 opponentPieces.AddRange(board.GetClonePiecesByColor(opponent.Color));
//                 // opponent's pieces on hand
//                 opponentPieces.AddRange(opponent.Pieces.ConvertAll(piece => piece.Clone()));

//                 // For every OPPONENT's piece
//                 foreach (var piece in opponentPieces)
//                 {
//                     HashSet<(ActionKind action, (int, int) to)> availableMoves = new();

//                     // // if this piece is on the board
//                     if (board.IsOnBoard(piece.ToString()) && !opponent.Pieces.Contains(piece))
//                     {
//                         // get its moving spots
//                         foreach (var to in piece.GetMovingSpots(ref board))
//                         {
//                             availableMoves.Add((ActionKind.Moving, to));
//                         }
//                     }

//                     // if opponent has pieces to play
//                     if (opponent.Pieces.Count > 0)
//                     {
//                         // get opponent's placing spots
//                         // availableMoves.AddRange(opponent.GetPlacingSpots(ref board));
//                         foreach (var to in opponent.GetPlacingSpots(ref board))
//                         {
//                             availableMoves.Add((ActionKind.Placing, to));
//                         }
//                     }

//                     // for every available move for current piece
//                     foreach (var availableMove in availableMoves)
//                     {
//                         Board temp = board.Clone();

//                         // make move
//                         if (availableMove.action == ActionKind.Moving && board.IsOnBoard(piece.ToString()))
//                         {
//                             board.MovePiece(piece, availableMove.to);
//                         }
//                         else if (availableMove.action == ActionKind.Placing && AI.Pieces.Contains(piece))
//                         {
//                             board.PlacePiece(opponent, piece, availableMove.to);
//                         }
//                                                                                                 // switch whose turn
//                         (int max, AIAction? move) = alpha_beta(ref board, ref AI, ref opponent, whoseTurn == Color.White ? Color.Black : Color.White, alpha, beta, depth + 1);

//                         if (max < minValue)
//                         {
//                             minValue = max;
//                             myMove = new AIAction(piece, availableMove);
//                         }

//                         // set game state back to what is was before making move
//                         // board.Undo();
//                         board = temp;

//                         // If alpha catches up to beta, kill kids
//                         if (minValue <= beta)
//                             return (minValue, myMove);

//                         // new value is greater than alpha? update alpha
//                         if (minValue < alpha)
//                             alpha = minValue;
//                     }
//                 }
//                 return (minValue, myMove);
//             }
//         }
//     }
// }

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
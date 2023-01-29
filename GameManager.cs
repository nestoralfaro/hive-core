using static GameCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace GameCore
{
    public class GameManager
    {
        public Board Board;
        // *******************************************
        // For testing
        public Dictionary<(int, int), Stack<Piece>> GetAllPieces() { return Board.Pieces; }
        public Dictionary<string, (int, int)> GetPiecePoint() { return Board._piece_point; }
        public Dictionary<Color, List<(int, int)>> GetColorPieces() { return Board._color_pieces; }
        // For testing
        // *******************************************

        public GameManager()
        {
            Board = new();
        }

        public static bool IsValidInput(Color color, Move move)
        {
            char c = char.ToLower(color.ToString()[0]);
            if (!move.ToString().Equals("invalid"))
            {
                if (move.ToString()[0] == c)
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException($"It is {color}'s turn.");
                }
            }
            {
                return false;
            }
        }

        public bool IsPlacingValid(Player player, Move move, Piece activePiece, (int x, int y) point)
        {
            if (player.TurnCount > 1)
            {
                if (player.TurnCount == 4 && player.HasNotPlayedQueen() && !activePiece.ToString().Equals($"{player.Color}Q1"))
                {
                    throw new ArgumentException("It is your 4th turn. You have to play your queen.");
                }

                List<(int, int)> availablePlacingSpots = player.GetPlacingSpots(Board.Pieces, Board._color_pieces, Board.IsAQueenSurrounded());
                if (availablePlacingSpots.Count != 0)
                {
                    if (availablePlacingSpots.Contains(point))
                    {
                        return true;
                    }
                    else
                    {
                        string availSpots = "";
                        foreach ((int x, int y) spot in availablePlacingSpots)
                        {
                            availSpots += $"({spot.x}, {spot.y})";
                        }
                        throw new ArgumentException($"Invalid placing for {move.MovingPiece}. Your available spots are: {availSpots}");
                    }
                }
                else
                {
                    throw new ArgumentException($"Piece {move.MovingPiece} has no valid placing spots.");
                }
            }
            else
            {
                // If the reference piece exist
                if (Board._piece_point.ContainsKey(move.DestinationPiece))
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException("Invalid Move: Make sure your piece of reference exists on the board.");
                }
            }
        }

        public bool IsPieceMoveOnBoardValid(Player player, Move move, Piece piece, (int, int) to)
        {
            if (player.TurnCount == 4 && player.HasNotPlayedQueen())
            {
                throw new ArgumentException("You have to play your queen before you are able to move your pieces.");
            }
            else
            {
                List<(int, int)> availableMovingSpots = Board.Pieces[piece.Point].Peek().GetMovingSpots(ref Board);
                if (availableMovingSpots.Count != 0)
                {
                    if (availableMovingSpots.Contains(to))
                    {
                        // piece.Point = to;
                        return true;
                    }
                    else
                    {
                        string availSpots = "";
                        foreach ((int x, int y) spot in availableMovingSpots)
                        {
                            availSpots += $"({spot.x}, {spot.y})";
                        }
                        throw new ArgumentException($"Invalid placing for {move.MovingPiece}. Your available spots are: {availSpots}");
                    }
                }
                else
                {
                    throw new ArgumentException($"Piece {move.MovingPiece} has no valid placing spots.");
                }
            }
        }

        private (int, int) GetNewPoint(Move move)
        {
            // It is the first piece being placed
            if (String.IsNullOrEmpty(move.DestinationSide))
            {
                return (0, 0);
            }
            else
            {
                if (Board._piece_point.ContainsKey(move.DestinationPiece))
                {
                    (int x, int y) = Board._piece_point[move.DestinationPiece];
                    (int x, int y) sideOffset = SIDE_OFFSETS[move.DestinationSide];
                    return (x + sideOffset.x, y + sideOffset.y);
                }
                else
                {
                    throw new ArgumentException($"Invalid placing for {move.DestinationPiece}. This piece does not exist on the board.");
                }
            }
        }

        private bool IsPlacingMove(Player player, Move move)
        {
            // if (!player.Pieces.Contains(move.MovingPiece))
            // {
            //     throw new ArgumentException($"Piece {move.MovingPiece} has already been played.");
            // }
            // return !_piece_point.ContainsKey(move.MovingPiece);
            return player.Pieces.Contains(move.MovingPiece) && !Board._piece_point.ContainsKey(move.MovingPiece);
        }

        private bool IsFirstMove(Piece piece)
        {
            // Nothing has been played
            if (Board.Pieces.Count == 0 && Board._piece_point.Count == 0)
            {
                if (piece.Insect == Insect.QueenBee)
                {
                    throw new ArgumentException("You may not play the queen as your first piece.");
                }
                return true;
            }
            else
            {
                throw new ArgumentException("Invalid Move: Make sure you include your side and reference piece–e.g., wQ1NWbB1");
            }
        }

        private void PrintAvailableMovesForThePiece(Player player, Piece piece)
        {
            // Improvement idea:
            // Parallelized both of these in the background (MovingPositions, PlacingPositions)
            Console.WriteLine("--------------------Available Moves--------------------");
            foreach ((int, int) point in piece.GetMovingSpots(ref Board))
            {
                Console.WriteLine(point);
            }
            Console.WriteLine("--------------------Available Placings--------------------");
            foreach ((int, int) point in player.GetPlacingSpots(Board.Pieces, Board._color_pieces, Board.IsAQueenSurrounded()))
            {
                Console.WriteLine(point);
            }
            Console.WriteLine("----------------------------------------------------------");
        }

        public bool MakeMove(ref Player player)
        {
            // temporarily disabled for testing
            // try
            // {
                Move move = Player.GetMove();
                if (IsValidInput(player.Color, move))
                {
                    (int, int) to = GetNewPoint(move);
                    Piece piece;
                    if (Board._piece_point.ContainsKey(move.MovingPiece))
                    {
                        (int, int) curPiecePoint = Board._piece_point[move.MovingPiece];
                        piece = Board.Pieces[curPiecePoint].Peek();
                    }
                    else
                    {
                        piece = new(move.MovingPiece, to);
                    }
                    if (move.IsMoveWithDestination())
                    {
                        if (IsPlacingMove(player, move))
                        {
                            if (IsPlacingValid(player, move, piece, to))
                            {
                                // // Add the new piece
                                // Board._AddPiece(to, piece);
                                // // Remove it from the player
                                // player.Pieces.Remove(move.MovingPiece);
                                Board.PlacePiece(player, piece, to);
                            }
                        }
                        else
                        {
                            // Possibly moving pieces already on the board
                            if (IsPieceMoveOnBoardValid(player, move, piece, to))
                            {
                                // // Move such existing piece
                                // // remove piece
                                // Board._RemovePiece(piece);
                                // // re-add it
                                // piece.Point = to;
                                // Board._AddPiece(to, piece);
                                // // PrintAvailableMovesForThePiece(piece);
                                Board.MovePiece(piece, to);
                            }
                            else
                            {
                                throw new ArgumentException("Invalid Move: Make sure your moving piece and piece of reference exist on the board.");
                            }
                        }
                    }
                    else
                    {
                        if (IsFirstMove(piece))
                        {
                            // // first piece on the board. Place it on the origin (0, 0)
                            // Board._AddPiece(to, piece);
                            // // Does not remove on the first turn
                            // player.Pieces.Remove(move.MovingPiece);
                            // // PrintAvailableMovesForThePiece(piece);
                            Board.PlacePiece(player, piece, to);
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid Move: Make sure you follow the appropriate notation, and it is an existing piece-e.g., wQ1");
                }
            // }
            // catch (ArgumentException ex)
            // {
            //     // something went wrong
            //     _PrintWarning(ex.Message);
            //     return false;
            // }
            return true;
        }

        public bool IsGameOver()
        {
            // Check if the game is over based on the game's rules
            // For example, check if a player has no more valid moves,
            // or if a player's queen bee has been surrounded
            if (Board._piece_point.ContainsKey("wQ1") && Board.Pieces.ContainsKey(Board._piece_point["wQ1"]) && Board.Pieces[Board._piece_point["wQ1"]].Peek().IsSurrounded())
            {
                PrintGreen("Black won!");
            }
            else if (Board._piece_point.ContainsKey("bQ1") && Board.Pieces.ContainsKey(Board._piece_point["bQ1"]) && Board.Pieces[Board._piece_point["bQ1"]].Peek().IsSurrounded())
            {
                PrintGreen("White won!");
            }
            else
            {
                return false;
            }
            return true;
        }

        public void Print()
        {
            // Print the board
            if (Board.Pieces.Count != 0)
            {
                Console.WriteLine("/*********************************/");
                Console.WriteLine("Current board state:");
                foreach (var entry in Board.Pieces)
                {
                    Console.WriteLine($"{entry.Key}: {entry.Value.Peek().Number} {entry.Value.Peek().Color} {entry.Value.Peek().Insect}");
                    //Print the neighbours
                    foreach (var neighbour in entry.Value.Peek().Neighbors)
                    {
                        Console.WriteLine($"{neighbour.Key}: {neighbour.Value}");
                    }
                }
                Console.WriteLine("/*********************************/");
            }
        }

        public void PrintFormatted()
        {
            if (Board.Pieces.Count != 0)
            {
                Dictionary<(int, int), bool> hasBeenPrinted = new();
                foreach (var entry in Board.Pieces)
                {
                    foreach (Piece piece in entry.Value)
                    {
                        if (!hasBeenPrinted.ContainsKey(piece.Point) || !hasBeenPrinted[piece.Point])
                        {
                            PrintAsHexagon(piece);
                            hasBeenPrinted[piece.Point] = true;
                        }
                    }
                }
            }
        }

        public static void PrintAsHexagon(Piece piece)
        {
            string NT = piece.Neighbors.ContainsKey("NT") ? ("\t\t" + piece.Neighbors["NT"].ToString() + Environment.NewLine + "\t\t-----------") : "\t\t-----------";
            string NW = piece.Neighbors.ContainsKey("NW") ? ("\t  " + piece.Neighbors["NW"].ToString() + "/") : "\t\t/";
            string SW = piece.Neighbors.ContainsKey("SW") ? ("\t  " + piece.Neighbors["SW"].ToString() +  "\\") : "\t\t\\";
            string ST = piece.Neighbors.ContainsKey("ST") ? ("\t\t-----------" + Environment.NewLine + "\t\t" + piece.Neighbors["ST"].ToString()) : "\t\t-----------";
            string SE = piece.Neighbors.ContainsKey("SE") ? ("\t /" + piece.Neighbors["SE"].ToString()) : "\t /";
            string NE = piece.Neighbors.ContainsKey("NE") ? ("\t \\" + piece.Neighbors["NE"].ToString()) : "\t \\";
            Console.WriteLine(NT);
            Console.WriteLine(NW + NE);
            Console.WriteLine($"\t\t{piece} {piece.Point}");
            Console.WriteLine(SW + SE);
            Console.WriteLine(ST);
            Console.WriteLine("*************************************");
            Console.WriteLine();
        }
    }
}
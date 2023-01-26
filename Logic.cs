using static GameCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace GameCore
{
    public class Logic
    {
        /// <summary>
        /// Hashmap { hashedPiece: piece }
        /// </summary>
        public Board Board; 
        private Dictionary<(int, int), Stack<Piece>> _point_stack { get { return Board._point_stack; } }
        private Dictionary<string, (int, int)> _piece_point { get { return Board._piece_point; } }
        private Dictionary<Color, List<Piece> > _color_pieces { get { return Board._color_pieces; } }

        // *******************************************
        // For testing
        public Dictionary<(int, int), Stack<Piece>> GetAllPieces() { return _point_stack; }
        public Dictionary<string, (int, int)> GetPiecePoint() { return _piece_point; }
        public Dictionary<Color, List<Piece>> GetColorPieces() { return _color_pieces; }
        // For testing
        // *******************************************

        public Logic()
        {
            Board = new();
        }

        private static void _PrintWarning(string warning)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(warning);
            Console.ResetColor();
            Console.WriteLine();
        }

        public static bool IsValidInput(char color, Move move)
        {
            if (!move.ToString().Equals("invalid"))
            {
                if (move.ToString()[0] == char.ToLower(color))
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException("It is " + (color == 'b' ? "Black's" : "White's") + " turn");
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

                List<(int, int)> availablePlacingSpots = activePiece.GetPlacingSpots(Board);
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
                if (_piece_point.ContainsKey(move.DestinationPiece))
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException("Invalid Move: Make sure your piece of reference exists on the board.");
                }
            }
        }

        public bool IsPieceMoveOnBoardValid(Move move, Piece piece)
        {
            List<(int, int)> availableMovingSpots = _point_stack[piece.Point].Peek().GetMovingSpots(Board);
            if (availableMovingSpots.Count != 0)
            {
                if (availableMovingSpots.Contains(piece.Point))
                {
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
            // return (
            //     _piece_point.ContainsKey(move.MovingPiece)
            //     && SIDE_OFFSETS.ContainsKey(move.DestinationSide)
            //     && _piece_point.ContainsKey(move.DestinationPiece)
            // );
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
                (int x, int y) = this._piece_point[move.DestinationPiece];
                (int x, int y) sideOffset = SIDE_OFFSETS[move.DestinationSide];
                return (x + sideOffset.x, y + sideOffset.y);
            }
        }

        private bool IsPlacingMove(Move move)
        {
            return !_piece_point.ContainsKey(move.MovingPiece);
        }

        private bool IsFirstMove()
        {
            // Nothing has been played
            if (Board._point_stack.Count == 0 && Board._piece_point.Count == 0)
            {
                return true;
            }
            else
            {
                throw new ArgumentException("Invalid Move: Make sure you include your side and reference piece–e.g., wQ1NWbB1");
            }
        }

        private void PrintAvailableMovesForThePiece(Piece piece)
        {
            // Improvement idea:
            // Parallelized both of these in the background (MovingPositions, PlacingPositions)
            Console.WriteLine("--------------------Available Moves--------------------");
            foreach ((int, int) point in piece.GetMovingSpots(Board))
            {
                Console.WriteLine(point);
            }
            Console.WriteLine("--------------------Available Placings--------------------");
            foreach ((int, int) point in piece.GetPlacingSpots(Board))
            {
                Console.WriteLine(point);
            }
            Console.WriteLine("----------------------------------------------------------");
        }

        public bool MakeMove(ref Player player)
        {
            try
            {
                Move move = Player.GetMove();
                if (IsValidInput(player.Color, move))
                {
                    (int, int) point = GetNewPoint(move);
                    Piece piece = new(move.MovingPiece, point);
                    if (move.IsMoveWithDestination())
                    {
                        if (IsPlacingMove(move))
                        {
                            if (IsPlacingValid(player, move, piece, point))
                            {
                                // Add the new piece
                                Board.AddPiece(point, piece);
                                // Remove it from the player
                                player.Pieces.Remove(move.MovingPiece);
                            }
                        }
                        else
                        {
                            // Possibly moving pieces already on the board
                            if (IsPieceMoveOnBoardValid(move, piece))
                            {
                                // Move such existing piece
                                // remove piece
                                Board.RemovePiece(piece);
                                // re-add it
                                Board.AddPiece(point, piece);
                                // PrintAvailableMovesForThePiece(piece);
                            }
                            else
                            {
                                throw new ArgumentException("Invalid Move: Make sure your moving piece and piece of reference exist on the board.");
                            }
                        }
                    }
                    else
                    {
                        if (IsFirstMove())
                        {
                            // first piece on the board. Place it on the origin (0, 0)
                            Board.AddPiece(point, piece);
                            // Does not remove on the first turn
                            player.Pieces.Remove(move.MovingPiece);
                            // PrintAvailableMovesForThePiece(piece);
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid Move: Make sure you follow the appropriate notation, and it is an existing piece-e.g., wQ1");
                }
            }
            catch (ArgumentException ex)
            {
                // something went wrong
                _PrintWarning(ex.Message);
                return false;
            }
            return true;
        }

        public bool IsGameOver()
        {
            // Check if the game is over based on the game's rules
            // For example, check if a player has no more valid moves,
            // or if a player's queen bee has been surrounded
            return (
                (_piece_point.ContainsKey("wQ1") && _point_stack.ContainsKey(_piece_point["wQ1"]) && _point_stack[_piece_point["wQ1"]].Peek().IsSurrounded())
                || (_piece_point.ContainsKey("bQ1") && _point_stack.ContainsKey(_piece_point["bQ1"]) && _point_stack[_piece_point["bQ1"]].Peek().IsSurrounded())
            );
        }


        public void Print()
        {
            // Print the board
            if (_point_stack.Count != 0)
            {
                Console.WriteLine("/*********************************/");
                Console.WriteLine("Current board state:");
                foreach (var entry in _point_stack)
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
            if (_point_stack.Count != 0)
            {
                Dictionary<(int, int), bool> hasBeenPrinted = new();
                foreach (var entry in _point_stack)
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
            string NT = piece.Neighbors.ContainsKey("NT") ? (piece.Neighbors["NT"].ToString() + System.Environment.NewLine + "-----------") : "\t-----------";
            string NW = piece.Neighbors.ContainsKey("NW") ? (piece.Neighbors["NW"].ToString() + "\t /") : "\t /";
            string SW = piece.Neighbors.ContainsKey("SW") ? (piece.Neighbors["SW"].ToString() + "\t \\") : "\t \\";
            string ST = piece.Neighbors.ContainsKey("ST") ? ("-----------" + System.Environment.NewLine + piece.Neighbors["ST"].ToString()) : "\t-----------";
            string SE = piece.Neighbors.ContainsKey("SE") ? ("\t / \t" + piece.Neighbors["SE"].ToString()) : "\t /";
            string NE = piece.Neighbors.ContainsKey("NE") ? ("\t \\ \t" + piece.Neighbors["NE"].ToString()) : "\t \\";

            Console.WriteLine(NT);
            Console.WriteLine(NW + NE);
            Console.WriteLine($"\t{piece} {piece.Point}");
            Console.WriteLine(SW + SE);
            Console.WriteLine(ST);
        }
    }
}
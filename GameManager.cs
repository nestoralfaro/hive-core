using static HiveCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace HiveCore
{
    public class GameManager
    {
        public Board Board;
        public GameManager() { Board = new(); }

        public static bool IsValidInput(Color color, Action move)
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

        public bool IsPlacingValid(Player player, (int x, int y) point)
        {
            return player.GetPlacingSpots(ref Board).Contains(point);
        }

        public bool IsMovingValid(Piece piece, (int, int) to)
        {
            return piece.GetMovingSpots(ref Board).Contains(to);
        }

        private (int, int) GetNewPoint(Action move)
        {
            // It is the first piece being placed
            if (String.IsNullOrEmpty(move.DestinationSide))
            {
                return (0, 0);
            }
            else
            {
                if (Board.IsOnBoard(move.DestinationPiece))
                {
                    (int x, int y) = Board.GetPointByString(move.DestinationPiece);
                    (int x, int y) sideOffset = SIDE_OFFSETS[move.DestinationSide];
                    return (x + sideOffset.x, y + sideOffset.y);
                }
                else
                {
                    throw new ArgumentException($"Invalid placing for {move.DestinationPiece}. This piece does not exist on the board.");
                }
            }
        }

        private bool IsPlacingMove(Player player, Action move, Piece piece)
        {
            return player.Pieces.Any(p => p.ToString().Equals(piece.ToString())) && !Board.IsOnBoard(move.MovingPiece);
        }

        private bool IsFirstMove(Piece piece)
        {
            if (Board.IsEmpty())
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

        public bool MakeMove(ref Player player)
        {
            try
            {
                Action move = Player.GetMove();
                if (IsValidInput(player.Color, move))
                {
                    (int, int) to = GetNewPoint(move);
                    Piece piece;
                    if (Board.IsOnBoard(move.MovingPiece))
                    {
                        (int, int) curPiecePoint = Board.GetPointByString(move.MovingPiece);
                        piece = Board.Pieces[curPiecePoint].Peek();
                    }
                    else
                    {
                        piece = new(move.MovingPiece)
                        {
                            Point = to
                        };
                    }
                    if (move.IsMoveWithDestination())
                    {
                        if (IsPlacingMove(player, move, piece))
                        {
                            if (IsPlacingValid(player, to))
                            {
                                Board.PlacePiece(player, piece, to);
                            }
                        }
                        else
                        {
                            if (IsMovingValid(piece, to))
                            {
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
                            Board.PlacePiece(player, piece, to);
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
            // This considers if a queen has been surrounded, but what about when the player has no more moves?
            if (Board.IsOnBoard("wQ1") && Board.GetClonePieceByStringName("wQ1").IsSurrounded())
            {
                PrintGreen("Black won!");
            }
            else if (Board.IsOnBoard("bQ1") && Board.GetClonePieceByStringName("bQ1").IsSurrounded())
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
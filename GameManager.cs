using static HiveCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace HiveCore
{
    public enum Color
    {
        Black,
        White
    }


    public class GameManager
    {
        public Board Board;
        public Color Winner;

        public GameManager()
        {
            Board = new();
        }

        public static Action GetMove()
        {
            Console.WriteLine("Enter move");
            string input = Console.ReadLine()!;
            return new Action(input);
        }

        public static bool IsValidInput(Action move, Color color)
        {
            Color playedColor = char.ToLower(move.MovingPiece[0]) == 'b' ? Color.Black : Color.White;
            if (playedColor != color)
            {
                throw new ArgumentException($"It is {color}'s turn.");
            }
            else
            {
                if (playedColor == color && !move.ToString().Equals("invalid"))
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException($"Invalid move.");
                }
            }
        }

        private bool IsPlacingMove(Piece piece, (int, int) to)
        {
            bool isValid = piece.GetPlacingSpots(ref Board).Contains(to);
            return !piece.IsOnBoard && isValid;
        }

        public bool IsMovingValid(Piece piece, (int, int) to)
        {
            return piece.GetMovingSpots(ref Board).Contains(to);
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
                    // Grab reference's piece point
                    (int x, int y) = Board.GetPointByString(move.DestinationPiece);
                    // Grab the offset point
                    (int x, int y) sideOffset = SIDE_OFFSETS[move.DestinationSide];
                    return (x + sideOffset.x, y + sideOffset.y);
                }
                else
                {
                    throw new ArgumentException($"Invalid placing for {move.DestinationPiece}. This piece does not exist on the board.");
                }
            }
        }

        public bool HumanMove(Color color)
        {
            try
            {
                Action move = GetMove();
                if (IsValidInput(move, color))
                {
                    (int, int) to = GetNewPoint(move);
                    Piece piece;
                    if (Board.IsOnBoard(move.MovingPiece))
                    {
                        // Grab the piece from the board
                        (int, int) curPiecePoint = Board.GetPointByString(move.MovingPiece);
                        piece = Board.Pieces[curPiecePoint].Peek();
                    }
                    else
                    {
                        piece = color == Color.White
                        ? Board.WhitePieces.First(p => p.ToString().Equals(move.MovingPiece))
                        : Board.BlackPieces.First(p => p.ToString().Equals(move.MovingPiece));
                    }

                    if (move.IsMoveWithDestination())
                    {
                        if (IsPlacingMove(piece, to))
                        {
                            Board.AddPiece(piece, to, false);
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
                            Board.AddPiece(piece, to, false);
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

        public void AIMove(Color color)
        {

        }

        public bool IsGameOver()
        {
            // This considers if a queen has been surrounded, but what about when the player has no more moves?
            if (Board.IsOnBoard("wQ1") && Board.GetRefPieceByStringName("wQ1").IsSurrounded())
            {
                Winner = Color.White;
            }
            else if (Board.IsOnBoard("bQ1") && Board.GetRefPieceByStringName("bQ1").IsSurrounded())
            {
                Winner = Color.Black;
            }
            else
            {
                return false;
            }
            return true;
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
using static HiveCore.Utils;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;
#pragma warning disable IDE1006 // Private members naming style

namespace HiveCore
{
    public class Player
    {
        public Color Color { get; set; }
        public List<Piece> Pieces { get; set; }
        public int TurnCount { get; set; }
        public bool HasNotPlayedQueen { get; set; }
        public Player(Color color)
        {
            Color = color;
            HasNotPlayedQueen = true;
            TurnCount = 1;
            char c = char.ToLower(color.ToString()[0]);
            Pieces = new List<Piece>()
            {
               new Piece($"{c}Q1"),
               new Piece($"{c}A1"),
               new Piece($"{c}A2"),
               new Piece($"{c}A3"),
               new Piece($"{c}B1"),
               new Piece($"{c}B2"),
               new Piece($"{c}G1"),
               new Piece($"{c}G2"),
               new Piece($"{c}G3"),
               new Piece($"{c}S1"),
               new Piece($"{c}S2"),
            };
        }

        // public void ReplaceWithState(Player player)
        // {
        //     HasNotPlayedQueen = player.HasNotPlayedQueen;
        //     TurnCount = player.TurnCount;
        //     Pieces.Clear();
        //     Pieces.AddRange(player.Pieces);
        // }

        public Player Clone()
        {
            return new Player(this.Color) {
                HasNotPlayedQueen = this.HasNotPlayedQueen,
                TurnCount = this.TurnCount,
                Pieces = this.Pieces.ConvertAll(piece => piece.Clone())
            };
        }

        public static Action GetMove()
        {
            Console.WriteLine("Enter move");
            string input = Console.ReadLine()!;
            return new Action(input!);
        }

        public List<(int, int)> GetPlacingSpots(ref Board board)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            // Maybe keep track of the visited ones with a hashmap and also pass it to the hasopponent neighbor?
            List<(int, int)> positions = new();

            // If nothing has been played
            if (board.IsEmpty())
            {
                // Origin is the only first valid placing spot
                return new List<(int, int)>(){(0, 0)};
            }
            // If there is only one piece on the board
            else if (board.Pieces.Count == 1)
            {
                // the available placing spots will be the such piece's surrounding spots
                return board.Pieces[(0, 0)].Peek().SpotsAround;
            }
            // Make sure the game is still going
            else if (!board.IsAQueenSurrounded())
            {
                // from here on out, only the spots that do not neighbor an opponent are valid  

                // iterate through the current player's pieces on the board
                foreach (Piece? piece in board.GetClonePiecesByColor(this.Color))
                {
                    if (piece != null)
                    {
                        // iterate through this piece's available spots
                        foreach ((int, int) spot in piece.SpotsAround)
                        {
                            //      Not been visited        It is not neighboring an opponent
                            if (!positions.Contains(spot) && !_HasOpponentNeighbor(spot, board.Pieces))
                            {
                                positions.Add(spot);
                            }
                        }
                    }
                }
            }

            stopwatch.Stop();
            PrintRed($"Generating Available Spots for Player {Color} took: {stopwatch.Elapsed.Milliseconds} ms");

            return positions;
        }


        private bool _HasOpponentNeighbor((int x, int y) point, Dictionary<(int, int), Stack<Piece>> pieces)
        {
            // foreach ((int, int) side in SIDE_OFFSETS.Values)
            for (int i = 0; i < 6; ++i)
            {
                (int, int) potentialOpponentNeighborPosition = (point.x + SIDE_OFFSETS_ARRAY[i].x, point.y + SIDE_OFFSETS_ARRAY[i].y);
                // If piece is on the board                                     And Is not the same color as the piece that is about to be placed
                // if (pieces.ContainsKey(potentialOpponentNeighborPosition) && pieces[potentialOpponentNeighborPosition].Peek().Color != this.Color)
                if (pieces.ContainsKey(potentialOpponentNeighborPosition) && pieces[potentialOpponentNeighborPosition].TryPeek(out Piece topPiece) && topPiece.Color != this.Color)
                {
                    // Has an opponent neighbor
                    return true;
                }
            }

            // Checked each side, and no opponent's pieces were found
            return false;
        }

        public virtual bool MakeMove(ref Board board, ref Player player)
        {
            throw new NotImplementedException();
        }
    }

    public class Action
    {
        private readonly string _move;
        public string MovingPiece { get; set; }
        public string DestinationSide { get; set; }
        public string DestinationPiece { get; set; }

        public override string ToString()
        {
            return _move;
        }

        private static string _formatPieceString(string piece)
        {
            char[] pieceAsChars = piece.ToCharArray();
            pieceAsChars[0] = char.ToLower(pieceAsChars[0]);
            pieceAsChars[1] = char.ToUpper(pieceAsChars[1]);
            return new string(pieceAsChars);
        }

        public Action (string input)
        {
            // To make the compiler happy so that it does not throw the
            // `Non-nullable property`
            MovingPiece = null!;
            DestinationPiece = null!;
            DestinationSide = null!;

            // Remove whitespaces
            input = Regex.Replace(input, @"\s+", "");
            //                  Case insensitive Not optional piece     Optional Side                Optional Piece
            const string validPattern = "^([wbWB])([ABGQSabgqs])([1-3])([NSns])?([TWEtwe])?([wbWB]?)([ABGQSabgqs]?)([1-3]?)$";
            if (input.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                _move = input.ToLower();
            }
            else if (Regex.IsMatch(input, validPattern))
            {
                MovingPiece = _formatPieceString(input[..3]);
                if (input.Length > 3)
                {
                    DestinationSide = input.Substring(3, 2).ToUpper();
                    DestinationPiece = _formatPieceString(input[5..]);
                }
                _move = input;
            }
            else
            {
                _move = "invalid";
            }
        }

        public bool IsMoveWithDestination()
        {
            const string validPattern = "^([wbWB])([ABGQSabgqs])([1-3])([NSns])([TWEtwe])([wbWB])([ABGQSabgqs])([1-3])$";
            return Regex.IsMatch(_move, validPattern);
        }
    }
    
    public class AIAction
    {
        public Piece Piece { get; set; }
        public (int x, int y) To { get; set; }
        public ActionKind Action {get; set;}
        public AIAction (Piece piece, (ActionKind action, (int x, int y) to) move)
        {
            Piece = piece;
            Action = move.action;
            To = move.to;
        }
    }

    public enum Color
    {
        Black,
        White
    }

    public enum ActionKind
    {
        Moving,
        Placing
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static GameCore.Utils;

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
        // private Dictionary<(int, int), Stack<Piece>> _point_stack;
        // private Dictionary<string, (int, int)> _piece_point;
        // private Dictionary<Color, List<Piece> > _color_pieces;


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

        private void _PrintWarning(string warning)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(warning);
            Console.ResetColor();
            Console.WriteLine();
        }

        public bool IsValidInput(Move move)
        {
            // This is a regex that could still be tricked, so we may have to check for that as well:
            // For instance, it would allow `wQ2`, but there really is only 1 Queen Bee.
                    //  Case insensitive Not optional piece       Optional Side       Optional Piece
            string validPattern = @"^([wb])([ABGQS])([1-3])([NS])?([TWE])?([wb]?)([ABGQS]?)([1-3]?)$";
            return Regex.IsMatch(move.ToString(), validPattern);
            // return true;
        }

        public bool IsPlacingValid(Move move)
        {
            return true;
        }

        public bool IsPieceOnBoardMoveValid(Move move, Piece piece)
        {
            return (
                _piece_point.ContainsKey(move.MovingPiece)
                && SIDE_OFFSETS.ContainsKey(move.DestinationSide)
                && _piece_point.ContainsKey(move.DestinationPiece)
            );
        }

        private (int, int) GetPoint(Move move)
        {
            // It is the first piece being placed
            if (String.IsNullOrEmpty(move.DestinationSide))
            {
                return (0, 0);
            }
            else
            {
                (int, int) referencePiece = this._piece_point[move.DestinationPiece];
                (int, int) delta = SIDE_OFFSETS[move.DestinationSide];
                return (referencePiece.Item1 + delta.Item1, referencePiece.Item2 + delta.Item2);
            }
        }

        private bool IsPlacingMove(Move move)
        {
            return !_piece_point.ContainsKey(move.MovingPiece);
        }

        private bool IsFirstMove()
        {
            // Nothing has been played
            return Board._point_stack.Count == 0 && Board._piece_point.Count == 0;
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
            Console.WriteLine("-------------------------------------------------------");

            Console.WriteLine("--------------------Available Placings--------------------");
            foreach ((int, int) point in piece.GetPlacingSpots(Board))
            {
                Console.WriteLine(point);
            }
            Console.WriteLine("----------------------------------------------------------");
        }

        public bool MakeMove(ref Player player)
        {
            Move move = player.GetMove();
            (int, int) point = GetPoint(move);
            Piece piece = new Piece(move.MovingPiece, point);
            try
            {
                if (IsValidInput(move))
                {
                    if (move.IsMoveWithDestination())
                    {
                        if (IsPlacingMove(move))
                        {
                            // If the reference piece exist
                            if (_piece_point.ContainsKey(move.DestinationPiece))
                            {
                                // Add the new piece
                                Board.AddPiece(point, piece);
                                // Remove it from the player
                                player.Pieces.Remove(move.MovingPiece);

                                PrintAvailableMovesForThePiece(piece);
                            }
                            else
                            {
                                throw new ArgumentException("Invalid Move: Make sure your piece of reference exists on the board.");
                            }
                        }
                        else
                        {
                            // Possibly moving pieces already on the board
                            if (IsPieceOnBoardMoveValid(move, piece))
                            {
                                // Move such existing piece
                                // remove piece
                                Board.RemovePiece(piece);
                                // re-add it
                                Board.AddPiece(point, piece);

                                PrintAvailableMovesForThePiece(piece);
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
                            PrintAvailableMovesForThePiece(piece);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid Move: Make sure you include your side and reference piece–e.g., wQ1NWbB1");
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid Move: Make sure you follow the appropriate notation-e.g., wQ1, and it is an existing piece");
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

        // Does not work
        public void PrintFormatted() {
            // Create a list to store the hexagons to print
            List<string> hexagons = new List<string>();

            // Iterate through each key-value pair in the dictionary
            foreach (KeyValuePair<(int, int), Stack<Piece>> kvp in _point_stack) {
                // Get the current hexagon and its neighbors
                (int, int) currentHex = kvp.Key;
                Piece currentPiece = kvp.Value.Peek();
                Dictionary<string, (int, int)> neighbors = currentPiece.Neighbors;

                // Create a string to store the current hexagon's output
                string hexagon = "";

                // Add the northwest neighbor if it exists
                if (neighbors.ContainsKey("*/")) {
                    hexagon += "  /";
                } else {
                    hexagon += "   ";
                }

                // Add the west neighbor if it exists
                if (neighbors.ContainsKey("*|")) {
                    hexagon += " \\";
                } else {
                    hexagon += "  ";
                }

                // Add the current hexagon's coordinates
                hexagon += $"\n |{currentHex.Item1},{currentHex.Item2}|";

                // Add the east neighbor if it exists
                if (neighbors.ContainsKey("|*")) {
                    hexagon += "\n  \\";
                } else {
                    hexagon += "\n   ";
                }

                // Add the southeast neighbor if it exists
                if (neighbors.ContainsKey("/*")) {
                    hexagon += " /";
                } else {
                    hexagon += "  ";
                }

                // Add the southwest neighbor if it exists
                if (neighbors.ContainsKey("*\\")) {
                    hexagon += " \\";
                } else {
                    hexagon += "  ";
                }

                // Add the northeast neighbor if it exists
                if (neighbors.ContainsKey("\\*")) {
                    hexagon += " /";
                } else {
                    hexagon += "  ";
                }

                // Add the current hexagon's output to the list of hexagons
                hexagons.Add(hexagon);
            }

            // Iterate through the list of hexagons and print each one
            for (int i = 0; i < hexagons.Count; i++) {
                Console.WriteLine(hexagons[i]);
            }
        }
    }
}
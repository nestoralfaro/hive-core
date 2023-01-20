using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameCore
{
    public class Board
    {
        /// <summary>
        /// Hashmap { hashedPiece: piece }
        /// </summary>
        private Dictionary<(int, int), Piece> _pieces;
        private Dictionary<string, (int, int)> _piece_coordinate;

        public Dictionary<string, (int, int)> _sides = new Dictionary<string, (int, int)>()
        {
            // Notice how each side is only valid if it adds up to an even number
            { "*/", (-1, 1) },   // [0] Northwest
            { "*|", (-2, 0) },   // [1] West
            { "*\\", (-1, -1) }, // [2] Southwest
            { "/*", (1, -1) },   // [3] Southeast
            { "|*", (2, 0) },    // [4] East
            { "\\*", (1, 1) },   // [5] Northeast
        };

        public Board()
        {
            _pieces = new Dictionary<(int, int), Piece>();
            _piece_coordinate = new Dictionary<string, (int, int)>();
        }

        private void _PrintWarning(string warning)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(warning);
            Console.ResetColor();
            Console.WriteLine();
        }

        public void AddPiece((int x, int y) point, Piece piece)
        {
            _pieces.Add(point, piece);
            _piece_coordinate.Add(piece.ToString(), point);
        }

        public void RemovePiece((int x, int y) point, string piece)
        {
            (int, int) piecePointToRemove = _piece_coordinate[piece];
            _pieces.Remove(piecePointToRemove);
            _piece_coordinate.Remove(piece);
        }

        public bool IsValidInput(Move move)
        {
            // This is a regex that could still be tricked, so we may have to check for that as well:
            // For instance, it would allow `wQ2`, but there really is only 1 Queen Bee.
                                //   Not optional piece       Optional Side       Optional Piece
                                                           //it fails here
            string validPattern = @"^([wb])([ABGQS])([1-3])([*|/*\|*\*=])?([wb]?)([ABGQS]?)([1-3]?)$";
            // return Regex.IsMatch(move.ToString(), validPattern);

            return true;
        }

        public bool IsPlacingValid(Move move)
        {
            return true;
        }

        public bool IsPieceOnBoardMoveValid(Move move, Piece piece)
        {
            return (
                _piece_coordinate.ContainsKey(move.MovingPiece)
                && _sides.ContainsKey(move.DestinationSide)
                && _piece_coordinate.ContainsKey(move.DestinationPiece)
            );
        }

        private (int, int) GetPoint(Move move)
        {
            // It is the first piece being placed
            if (String.IsNullOrEmpty(move.DestinationSide)) return (0, 0);
            else
            {
                (int, int) referencePiece = _piece_coordinate[move.DestinationPiece];
                (int, int) delta = _sides[move.DestinationSide];
                return (referencePiece.Item1 + delta.Item1, referencePiece.Item2 + delta.Item2);
            }
        }

        private void PopulateNeighborsFrom((int x, int y) point)
        {
            _pieces[point].Neighbors.Clear();
            foreach (KeyValuePair<string, (int, int)> side in _sides)  
            {
                // bool IsNeighbour = (point % side == (0, 0));
                (int, int) neighborPoint = (point.x + side.Value.Item1, point.y + side.Value.Item2);
                bool neighbourExists = _pieces.ContainsKey(neighborPoint);
                if (neighbourExists)
                {
                    _pieces[point].Neighbors[side.Key] = neighborPoint;
                }
            }
        }

        private void UpdateAllNeighbors()
        {
            foreach (var entry in _pieces)
            {
                PopulateNeighborsFrom(entry.Key);
            }
        }

        private bool IsPlacingMove(Move move)
        {
            return !_piece_coordinate.ContainsKey(move.MovingPiece);
        }

        private bool IsFirstMove()
        {
            // No thing has been played
            return _pieces.Count == 0 && _piece_coordinate.Count == 0;
        }

        public bool MakeMove(ref Player player)
        {
            Move move = player.GetMove();
            (int, int) point = GetPoint(move);
            Piece piece = new Piece(move.MovingPiece);
            try
            {
                if (IsValidInput(move))
                {
                    if (move.IsMoveWithDestination())
                    {
                        if (IsPlacingMove(move))
                        {
                            // If the reference piece exist
                            if (_piece_coordinate.ContainsKey(move.DestinationPiece))
                            {
                                // Add the new piece
                                AddPiece(point, piece);
                                // Remove it from the player
                                player.Pieces.Remove(move.MovingPiece);
                                UpdateAllNeighbors();
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
                                RemovePiece(point, piece.ToString());
                                // re-add it
                                AddPiece(point, piece);
                                UpdateAllNeighbors();
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
                            AddPiece(point, piece);
                            // Does not remove on the first turn
                            player.Pieces.Remove(move.MovingPiece);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid Move: Make sure you include your side and reference piece–e.g., wQ1*/bB1");
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
                (_piece_coordinate.ContainsKey("wQ1") && _pieces[_piece_coordinate["wQ1"]].IsSurrounded())
                || (_piece_coordinate.ContainsKey("bQ1") && _pieces[_piece_coordinate["wQ1"]].IsSurrounded())
            );
        }

        public void Print()
        {
            // Print the board
            Console.WriteLine("/*********************************/");
            Console.WriteLine("Current board state:");
            foreach (var entry in _pieces)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value.Insect} {entry.Value.Number}");
                //Print the neighbours
                foreach (var neighbour in entry.Value.Neighbors)
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
            foreach (KeyValuePair<(int, int), Piece> kvp in _pieces) {
                // Get the current hexagon and its neighbors
                (int, int) currentHex = kvp.Key;
                Piece currentPiece = kvp.Value;
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
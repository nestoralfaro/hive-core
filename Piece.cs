using System;
using System.Collections;
using System.Collections.Generic;
using static GameCore.Utils;
#pragma warning disable IDE1006 // Private members naming style
#nullable enable

namespace GameCore
{
    // Because the board is used to often, we should consider either 
    // inheriting from "Board" or making it publicly available.
    // Maybe it should be just called game_state.
    // Similarly with _sides_offset. Given that it is a constant, it should be global instead
    public class Piece
    {
        private const int _MANY_SIDES = 6;
        private const int _SPIDER_MAX_STEP_COUNT = 3;
        private const int _ANT_MAX_STEP_COUNT = -1;
        private const int _QUEENBEE_MAX_STEP_COUNT = 1;
        private const int _GRASSHOPPER_MAX_STEP_COUNT = 1;
        private const int _BEETLE_MAX_STEP_COUNT = 1;

        public Dictionary<string, (int, int)> Neighbors { get; set; }
        public Dictionary<string, (int, int)> Sides { get; set; }
        public List<(int, int)> SpotsAround { get { return Sides.Except(Neighbors).Select(side => side.Value).ToList(); } }

        public (int x, int y) Point { get; set; }
        public Insect Insect { get; set; }
        public Color Color { get; set; }
        public int Number { get; set; }

        private Board _board;

        private Dictionary<string, (int, int)> _piece_point { get; set; }
        private Dictionary<(int, int), Stack<Piece>> _point_stack { get; set; }
        private Dictionary<Color, List<Piece> > _color_pieces { get; set; }
        private string _piece;

        public Piece(string piece, (int, int) piecePoint)
        {
            _piece = piece;
            Color = piece[0] == 'b' ? Color.Black : Color.White;
            Insect = piece[1] == 'Q'
                        ? Insect.QueenBee
                        : piece[1] == 'B'
                        ? Insect.Beetle
                        : piece[1] == 'G'
                        ? Insect.Grasshopper
                        : piece[1] == 'S'
                        ? Insect.Spider
                        // piece[1] == 'A'
                        : Insect.Ant;
            Number = piece[2] - '0';
            Neighbors = new Dictionary<string, (int, int)>();
            Point = piecePoint;

            Sides = new Dictionary<string, (int, int)>()
            {
                // These values may not need to be hardcoded.
                // However, hardcoding them may make them more efficient
                { "NT", (piecePoint.Item1 + (1), piecePoint.Item2 + (1)) },    // [0] North
                { "NW", (piecePoint.Item1 + (-1), piecePoint.Item2 + (1)) },   // [1] Northwest
                { "SW", (piecePoint.Item1 + (-2), piecePoint.Item2 + (0)) },   // [2] Southwest
                { "ST", (piecePoint.Item1 + (-1), piecePoint.Item2 + (-1)) },  // [3] South
                { "SE", (piecePoint.Item1 + (1), piecePoint.Item2 + (-1)) },   // [4] Southeast
                { "NE", (piecePoint.Item1 + (2), piecePoint.Item2 + (0)) },    // [5] Northeast
            };
        }

        public bool IsSurrounded()
        {
            return Neighbors.Count == _MANY_SIDES;
        }

        // isValidPath?

        public List<(int, int)> GetMovingSpots(Board board)
        {
            _board = board;
            _point_stack = board._point_stack;
            _piece_point = board._piece_point;
            _color_pieces = board._color_pieces;

            switch (Insect)
            {
                case Insect.Ant:
                    return _GetAntMovingSpots();
                case Insect.Beetle:
                    return _GetBeetleMovingSpots();
                case Insect.Grasshopper:
                    return _GetGrasshopperMovingSpots();
                case Insect.Spider:
                    return _GetSpiderMovingSpots();
                case Insect.QueenBee:
                    return _GetQueenMovingSpots();
                default:
                    // this is an invalid position
                    return new List<(int, int)>() { (1, 2) };
            }
        }

        private bool _IsClockwiseTurn((int x, int y) p1, (int x, int y) p2, (int x, int y) p3)
        {
            // If the area of the parallelogram is negative
            return (p2.x - p1.x) * (p3.y - p1.y) - (p2.y - p1.y) * (p3.x - p1.x) < 0;
        }

        private bool _HasAtLeastOneNeighbor((int x, int y) point)
        {
            // This point cannot be itself
            if (point.x == Point.x && point.y == Point.y)
                return false;

            foreach ((int x, int y) side in SIDE_OFFSETS.Values)
            {
                (int x, int y) neighborPosition = (point.x + side.x, point.y + side.y);
                // If the neighbor is not myself                                               AND If the neighbor exists on the board
                if ((this.Point.x != neighborPosition.x || this.Point.y != neighborPosition.y) && _point_stack.ContainsKey(neighborPosition))
                {
                    // This position has at least one neighbor connected to the other pieces,
                    // which would not break the hive
                    return true;
                }
            }

            // Checked each side, and no pieces were found,
            // therefore `point` is a position that would break the hive
            return false;
        }

        private List<(int, int)> _GetAntMovingSpots()
        {
            // Find convex hull with Graham's Scan algorithm
            List<(int, int)> convexHull = new List<(int, int)>();
            List<(int, int)> activePoints = new List<(int, int)>(); 

            foreach (KeyValuePair<(int, int), Stack<Piece>> p in _point_stack)
            {
                if (!activePoints.Contains(p.Key))
                        activePoints.Add(p.Key);
                // Include also the neighboring sides for each piece
                foreach ((int, int) side in p.Value.Peek().Sides.Values)
                {
                    if (!activePoints.Contains(side))
                        activePoints.Add(side);
                }
            }

            List<(int, int)> spotsInsideConvexHull = new List<(int, int)>();

            // Find point with lowest y-coordinate–i.e., the bottom most point
            (int, int) start = activePoints.OrderBy(p => p.Item2).First();
            convexHull.Add(start);

            // Sort the other points by the angle with respect to the lowest y-point
            IOrderedEnumerable<(int, int)> sortedPoints = activePoints.Where(p => p != start).OrderBy(p => Math.Atan2(p.Item2 - start.Item2, p.Item1 - start.Item1));

            // Iterate through sorted points and add/remove as necessary
            foreach ((int, int) p in sortedPoints)
            {
                while (convexHull.Count > 1 && _IsClockwiseTurn(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], p))
                {

                    // Validate each position
                    // Each position must have at least one neighbor–i.e., one piece in one of its sides, that way the hive does not break
                    // That is, before removing this particular point, see if it has neighbors, if it does, then it is possible still valid
                    // Remove the points that create a clockwise turn

                    (int, int) spotInsideConvexHull = convexHull[convexHull.Count - 1];
                    if (!spotsInsideConvexHull.Contains(spotInsideConvexHull) && _HasAtLeastOneNeighbor(spotInsideConvexHull) && !_point_stack.ContainsKey(spotInsideConvexHull))
                    {
                        spotsInsideConvexHull.Add(spotInsideConvexHull);
                    }
                    convexHull.RemoveAt(convexHull.Count - 1);
                }
                convexHull.Add(p);
            }

            // Add the missing points
            convexHull.AddRange(spotsInsideConvexHull);

            // Filter out invalid points
            return convexHull.Where(side => _HasAtLeastOneNeighbor(side)).ToList();
        }

        private List<(int, int)> _GetBeetleMovingSpots()
        {
            var validMoves = new List<(int, int)>();
            foreach ((int, int) side in Sides.Values)
            {
                // Filter out the sides that would break the hive
                if (_IsValidPath(this.Point, side, true))
                {
                    validMoves.Add(side);
                }
            }
            return validMoves;
        }

        private List<(int, int)> _GetGrasshopperMovingSpots()
        {
            List<(int x, int y)> positions = new List<(int x, int y)>();
            foreach ((int x, int y) sideOffset in SIDE_OFFSETS.Values)
            {
                (int x, int y) nextSpot = (this.Point.x + sideOffset.x, this.Point.y + sideOffset.y);
                bool firstIsValid = false;

                // Keep hopping over pieces
                while (_point_stack.ContainsKey(nextSpot))
                {
                    nextSpot = (nextSpot.x + sideOffset.x, nextSpot.y + sideOffset.y);
                    firstIsValid = true;
                }

                if (firstIsValid)
                {
                    // until you find a spot
                    positions.Add(nextSpot);
                }
            }
            return positions;
        }

        private List<(int, int)> _GetQueenMovingSpots()
        {
            List<(int, int)> spots = new List<(int, int)>();
            foreach ((int, int) spot in SpotsAround)
            {
                // It is not busy and is valid
                if (_IsValidPath(Point, spot))
                {
                    spots.Add(spot);
                }
            }
            return spots;
        }

        private bool _IsFreedomOfMovement ((int x, int y) from, (int x, int y) to)
        {
            (int x, int y) offset = (to.x - from.x, to.y - from.y);
            int index = SIDE_OFFSETS_LIST.IndexOf(offset); // direction we're going

            (int x, int y) leftOffset = index == 0 ? SIDE_OFFSETS_LIST[5] : SIDE_OFFSETS_LIST[index - 1];
            (int x, int y) rightOffset = index == 5 ? SIDE_OFFSETS_LIST[0] : SIDE_OFFSETS_LIST[index + 1];

            (int x, int y) peripheralLeftSpot = (from.x + leftOffset.x, from.y + leftOffset.y);  
            (int x, int y) peripheralRightSpot = (from.x + rightOffset.x, from.y + rightOffset.y);

            bool eitherPeripheralSpotIsNotItself = (peripheralLeftSpot.x != Point.x || peripheralLeftSpot.y != Point.y) && (peripheralRightSpot.x != Point.x || peripheralRightSpot.y != Point.y);
            bool onlyOneSpotIsOpen = (_point_stack.ContainsKey(peripheralLeftSpot) && !_point_stack.ContainsKey(peripheralRightSpot)) || (!_point_stack.ContainsKey(peripheralLeftSpot) && _point_stack.ContainsKey(peripheralRightSpot));

            // the piece is physically able to move to such spot
            return eitherPeripheralSpotIsNotItself && onlyOneSpotIsOpen;
        }

        // Needs fixing
        private bool _DoesNotBreakHive((int x, int y) from, (int x, int y) to)
        {
            // ********************* Checking for broken hive ********************* 
            Piece oldPieceSpot = new Piece(ToString(), Point);
            Piece newPieceSpot = new Piece(ToString(), to);
            _board.RemovePiece(this);
            _board.AddPiece(to, newPieceSpot);

            // Check if there is at least one common neighbor
            // if (oldPieceSpot.Neighbors.Where newPieceSpot.Neighbors)
            // {
            // }

            if (!_board.IsAllConnected())
            {
                // Put it back
                _board.RemovePiece(newPieceSpot);
                _board.AddPiece(oldPieceSpot.Point, oldPieceSpot);

                // it breaks the hive
                return false;
            }
            // Put it back
            _board.RemovePiece(newPieceSpot);
            _board.AddPiece(oldPieceSpot.Point, oldPieceSpot);
            return true;
            // ********************* Checking for broken hive ********************* 
        }

        private bool _IsValidPath((int x, int y) from, (int x, int y) to, bool isBeetle = false)
        {
            //      Only beetle can crawl on top of                 One Hive Rule                       Physically Fits
            return (isBeetle || !_point_stack.ContainsKey(to)) && _DoesNotBreakHive(from, to) && _IsFreedomOfMovement(from, to);
        }

        private void _DFS(ref List<(int x, int y)> positions, ref Dictionary<(int x, int y), bool> visited, (int x, int y) curSpot, int curDepth, int maxDepth)
        {
            if (curDepth == maxDepth)
            {
                // No one is at that position
                if (!_point_stack.ContainsKey(curSpot))
                {
                    visited[curSpot] = true;
                    positions.Add(curSpot);
                }
                return;
            }
            
            visited[curSpot] = true;

            foreach ((int x, int y) offset in SIDE_OFFSETS.Values)
            {
                (int x, int y) nextSpot = (curSpot.x + offset.x, curSpot.y + offset.y);
                bool hasNotBeenVisited = !visited.ContainsKey(nextSpot) || (visited.ContainsKey(nextSpot) && !visited[nextSpot]);
                if (hasNotBeenVisited && _IsValidPath(curSpot, nextSpot))
                {
                    _DFS(ref positions, ref visited, nextSpot, curDepth + 1, maxDepth);
                }
            }
        }

        private List<(int, int)> _GetSpiderMovingSpots()
        {
            List<(int x, int y)> positions = new List<(int x, int y)>();
            Dictionary<(int x, int y), bool> visited = new Dictionary<(int x, int y), bool>();
            foreach ((int x, int y) side in SpotsAround)
            {
                bool hasNotBeenVisited = !visited.ContainsKey(side) || (visited.ContainsKey(side) && !visited[side]);
                if (hasNotBeenVisited && _IsValidPath(Point, side))
                {
                    _DFS(ref positions, ref visited, side, 1, _SPIDER_MAX_STEP_COUNT);
                }
            }
            return positions;
        }

        private bool _HasOpponentNeighbor((int, int) point)
        {
            foreach ((int, int) side in SIDE_OFFSETS.Values)
            {
                (int, int) potentialOpponentNeighborPosition = (point.Item1 + side.Item1, point.Item2 + side.Item2);
                // If piece is on the board                             AND is not the same color as the piece that is about to be placed
                if (_point_stack.ContainsKey(potentialOpponentNeighborPosition) && _point_stack[potentialOpponentNeighborPosition].Peek().Color != this.Color)
                {
                    // Has an opponent neighbor
                    return true;
                }
            }

            // Checked each side, and no opponent's pieces were found
            return false;
        }

        public List<(int, int)> GetPlacingSpots(Board board)
        {
            _board = board;
            _point_stack = board._point_stack;
            _piece_point = board._piece_point;
            _color_pieces = board._color_pieces;

            List<(int, int)> positions = new List<(int, int)>();

            // iterate through the current player's color's pieces
            foreach (Piece piece in board._color_pieces[this.Color])
            {
                // iterate through this piece's available sides
                foreach ((int, int) spot in piece.SpotsAround)
                {
                    //      Not been visited            No one is there                   Does not have neighbor opponent
                    if (!positions.Contains(spot) && !_point_stack.ContainsKey(spot) && !_HasOpponentNeighbor(spot))
                    {
                            positions.Add(spot);
                    }
                }
            }

            return positions;
        }

        public int GetManyNeighbors()
        {
            return Neighbors.Count;
        }

        public string ToString()
        {
            return _piece;
        }

    }

    public enum Insect
    {
        QueenBee,
        Beetle,
        Grasshopper,
        Spider,
        Ant,
    }
}
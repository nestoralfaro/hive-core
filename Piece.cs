using System;
using System.Collections;
using System.Collections.Generic;
#nullable enable

namespace GameCore
{
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
        public List<(int, int)> GetAvailableSides() { return Sides.Except(Neighbors).Select(side => side.Value).ToList(); }

        public (int, int) Point { get; set; }
        public Insect Insect { get; set; }
        public Color Color { get; set; }
        public int Number { get; set; }

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
                // These values may not need to be hardcoded. Maybe if the
                // sides_offsets were passed to the constructor, that way
                // if we ever need to change, we only need to do it from one place.
                // However, hardcoding them may make them more efficient
                { "*/", (piecePoint.Item1 + (-1), piecePoint.Item2 + (1))},     // [0] Northwest
                { "*|", (piecePoint.Item1 + (-2), piecePoint.Item2 + (0))},     // [1] West
                { "*\\", (piecePoint.Item1 + (-1), piecePoint.Item2 + (-1))},   // [2] Southwest
                { "/*", (piecePoint.Item1 + (1), piecePoint.Item2 + (-1))},     // [3] Southeast
                { "|*", (piecePoint.Item1 + (2), piecePoint.Item2 + (0))},      // [4] East
                { "\\*", (piecePoint.Item1 + (1), piecePoint.Item2 + (1))},     // [5] Northeast
            };
        }

        public bool IsSurrounded()
        {
            // TODO: Maybe validate such piece?
            return Neighbors.Count == _MANY_SIDES;
        }

        public List<(int, int)> GetMovingPositions(Dictionary<(int, int), Piece> board, Dictionary<string, (int, int)>.ValueCollection sideOffset)
        {
            switch (Insect)
            {
                case Insect.Ant:
                    return _GetAntMovingPositions(board, sideOffset);
                case Insect.Beetle:
                    return _GetBeetleMovingPositions(board, sideOffset);
                case Insect.Grasshopper:
                    return _GetGrasshopperMovingPositions(board, sideOffset);
                case Insect.Spider:
                    return _GetSpiderMovingPositions(board, sideOffset);
                case Insect.QueenBee:
                    return _GetQueenBeeMovingPositions(board, sideOffset);
                default:
                    // this is an invalid position
                    return new List<(int, int)>() { (1, 2) };
            }
        }

        private bool _IsClockwiseTurn((int, int) p1, (int, int) p2, (int, int) p3)
        {
            // If the area of the parallelogram is negative
            return (p2.Item1 - p1.Item1) * (p3.Item2 - p1.Item2) - (p2.Item2 - p1.Item2) * (p3.Item1 - p1.Item1) < 0;
        }

        private bool _HasAtLeastOneNeighbor((int, int) point, Dictionary<(int, int), Piece> board, Dictionary<string, (int, int)>.ValueCollection sideOffset)
        {
            foreach ((int, int) side in sideOffset)
            {
                (int, int) neighborPosition = (point.Item1 + side.Item1, point.Item2 + side.Item2);
                // If the neighbor is not myself  AND If the neighbor exists on the board
                if (this.Point != neighborPosition && board.ContainsKey(neighborPosition))
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

        /**
        An idea of how this function could be implemented:
        Find the convex hull, then go through each point, and filter out only the valid sides–e.g., NW, W, SW, SE, E, and NE.
        I think that if you do that filtering, you would get a good amount of the potential positions. From there it would
        be a matter of validating each position–i.e., making sure they are not gates. 

        Note: Remember that to evaluate each side you just need to add the right value that maps to each side.
        For instance, given point x = (-2, 0), to know its Southwest side, just add the west side, which is (-1, -1).
        Therfore, x's Southwest side is (-2 + (-1), 0 + (-1)) = (-3, -1) (double check with the chart sent to discord)
        **/
        private List<(int, int)> _GetAntMovingPositions(Dictionary<(int, int), Piece> board, Dictionary<string, (int, int)>.ValueCollection sideOffset )
        {
            /**
            TODO: The convex hull needs to make sure that the moves:
            1. do not break the hive!
            2. It is not a gate (a position where there is only one available side for a neighbor)
            **/

            // Find convex hull with Graham's Scan algorithm
            List<(int, int)> convexHull = new List<(int, int)>();
            List<(int, int)> activePoints = new List<(int, int)>(); 

            foreach (KeyValuePair<(int, int), Piece> p in board)
            {
                if (!activePoints.Contains(p.Key))
                    activePoints.Add(p.Key);
                // Include also the neighboring sides for each piece
                foreach ((int, int) side in p.Value.Sides.Values)
                {
                    if (!activePoints.Contains(side))
                        activePoints.Add(side);
                }
            }

            // 1. Find point with lowest y-coordinate–i.e., the bottom most point
            (int, int) start = activePoints.OrderBy(p => p.Item2).First();
            convexHull.Add(start);

            // 2. Sort the other points by angle relative to the x-axis
            IOrderedEnumerable<(int, int)> sortedPoints = activePoints.Where(p => p != start).OrderBy(p => Math.Atan2(p.Item2 - start.Item2, p.Item1 - start.Item1));

            // 3. Iterate through sorted points and add/remove as necessary
            foreach ((int, int) p in sortedPoints)
            {
                while (convexHull.Count > 1 && _IsClockwiseTurn(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], p))
                {

                    // Validate each position
                    // Each position must have at least one neighbor–i.e., one piece in one of its sides, that way the hive does not break
                    // That is, before removing this particular point, see if it has neighbors, if it does, then it is possible still valid

                    // Remove the points that create a clockwise turn
                    convexHull.RemoveAt(convexHull.Count - 1);
                }
                convexHull.Add(p);
            }

            // 4. return the convexHull
            return convexHull.Where(side => _HasAtLeastOneNeighbor(side, board, sideOffset)).ToList();
        }

        /**
        An idea of how this function could be implemented:
        Just return all the neighbors for this beetle by adding the sides from the hashmap, and validate each one of them.
        Remember that the beetle can also get on top. That means that we would have to add another piece to a particulate
        element on the hashmap.

        For instance, a hashmap with a beetle on top of a piece could look like this:
        {
            (0, 0):
                Piece({Insect: Ant, Number: 1, Color: White}),
                Piece({Insect: Beetle, Number: 2, Color: Black}), -->   Maybe the last piece is always the beetle?
                                                                        Up to your engineering skills to figure that one out
        }

        One tricky part maybe populating the neighbors property, but it really should be just adding the ones that
        the other pieces at that position have, without repetition of course.

        Maybe something like:
            validPositions = []
            foreach (var side in sides)
                validPositions.Add(ValidateBeetleMoves(piece_positions[piece] + side))
        **/
        private List<(int, int)> _GetBeetleMovingPositions(Dictionary<(int, int), Piece> board, Dictionary<string, (int, int)>.ValueCollection sideOffset)
        {
            // Filter out the sides that would break the hive
            List<(int, int)> validMoves = this.GetAvailableSides().Where(side => _HasAtLeastOneNeighbor(side, board, sideOffset)).ToList(); 

            // Because it is a beetle, add its existing neighbors too, because it can get on top of them
            validMoves.AddRange(this.Neighbors.Select(side => side.Value).ToList());
            
            return validMoves;
        }


        /**
        An idea of how this function could be implemented:
        Keep adding to a side until you find an available spot. Maybe like:
            validPositions = []
            foreach (var side in sides)
                potentialPosition = piece_positions(piece) + side

                // if it does not exist on the "board", then it is an available spot 
                while (board.ContainsKey(potentialPosition))
                    potentialPosition + side
                validPositions.Add(potentialPosition)
        **/
        private List<(int, int)> _GetGrasshopperMovingPositions(Dictionary<(int, int), Piece> board, Dictionary<string, (int, int)>.ValueCollection sideOffset)
        {
            return new List<(int, int)>() {(0, 0)};
        }

        /**
        An idea of how this function could be implemented:
        Similar to the beetle, just return the sides by adding once to each of them.
        Something like:
            positions = []
            foreach (var side in sides)
                positions.Add(ValidateQueenBeePosition(piece_positions[piece]));
        **/
        private List<(int, int)> _GetQueenBeeMovingPositions(Dictionary<(int, int), Piece> board, Dictionary<string, (int, int)>.ValueCollection sideOffset)
        {
            return new List<(int, int)>() {(0, 0)};
        }

        /**
        An idea of how this function could be implemented:
        Maybe doing DFS on its sides and validating each of them?
        Something like:
            positions = []
            foreach (var side in sides)
                positions.Add(ValidateSpiderPosition(DFS(piece_positions[piece])));
        **/
        private List<(int, int)> _GetSpiderMovingPositions(Dictionary<(int, int), Piece> board, Dictionary<string, (int, int)>.ValueCollection sideOffset)
        {
            return new List<(int, int)>() {(0, 0)};
        }

        private bool _HasOpponentNeighbor((int, int) point, Dictionary<string, (int, int)>.ValueCollection sideOffset, Dictionary<(int, int), Piece> board)
        {
            foreach ((int, int) side in sideOffset)
            {
                (int, int) potentialOpponentNeighborPosition = (point.Item1 + side.Item1, point.Item2 + side.Item2);
                // If piece is on the board                             AND is not the same color as the piece that is about to be placed
                if (board.ContainsKey(potentialOpponentNeighborPosition) && board[potentialOpponentNeighborPosition].Color != this.Color)
                {
                    // Has an opponent neighbor
                    return true;
                }
            }

            // Checked each side, and no opponent's pieces were found
            return false;
        }

        public List<(int, int)> GetPlacingPositions(Dictionary<Color, List<Piece> > color_pieces, Dictionary<(int, int), Piece> board, Dictionary<string, (int, int)>.ValueCollection sideOffsets)
        {
            List<(int, int)> positions = new List<(int, int)>();

            // iterate through the current player's color's pieces
            foreach (Piece piece in color_pieces[this.Color])
            {
                // iterate through this piece's available sides
                foreach ((int, int) side in piece.GetAvailableSides())
                {
                    // if it does not neighbor with an opponent's piece
                    if (!_HasOpponentNeighbor(side, sideOffsets, board))
                        // If it is not already there
                        if (!positions.Contains(side))
                            // It is a valid placing position, so add it
                            positions.Add(side);
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
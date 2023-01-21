using System;
using System.Collections;
using System.Collections.Generic;

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
        public Insect Insect { get; set; }
        public Color Color { get; set; }
        public int Number { get; set; }

        private string _piece;

        public Piece(string piece)
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
        }

        public bool IsSurrounded()
        {
            // TODO: Maybe validate such piece?
            return Neighbors.Count == _MANY_SIDES;
        }

        public List<(int, int)> GetMovingPositions(string piece, Dictionary<string, (int, int)> piece_positions, Dictionary<(int, int), Piece> Board)
        {
            switch (Insect)
            {
                case Insect.Ant:
                    return GetAntMovingPositions();
                case Insect.Beetle:
                    return GetBeetleMovingPositions();
                case Insect.Grasshopper:
                    return GetGrasshopperMovingPositions();
                case Insect.Spider:
                    return GetSpiderMovingPositions();
                case Insect.QueenBee:
                    return GetQueenBeeMovingPositions();
            }
        }

        private bool _IsRightTurn((int, int) p1, (int, int) p2, (int, int) p3)
        {
            return (p2.Item1 - p1.Item1) * (p3.Item2 - p1.Item2) - (p2.Item2 - p1.Item2) * (p3.Item1 - p1.Item1) < 0;
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
        public List<(int, int)> GetAntMovingPositions(string piece, Dictionary<string, (int, int)> piece_positions, Dictionary<(int, int), Piece> board)
        {
            // Find convex hull with Graham's Scan algorithm
            List<(int, int)> convexHull = new List<(int, int)>();

            // 1. Find point with lowest y-coordinate–i.e., the bottom most point
            (int, int) start = board.Keys.OrderBy(p => p.Item2).First();
            convexHull.Add(start);

            // 2. Sort the other points by angle relative to the x-axis
            var sortedPoints = board.Keys.Where(p => p != start).OrderBy(p => Math.Atan2(p.Item2 - start.Item2, p.Item1 - start.Item1));

            // 3. Iterate through sorted points and add/remove as necessary
            foreach ((int, int) p in sortedPoints)
            {
                while (convexHull.Count > 1 && _IsRightTurn(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], p))
                {
                    convexHull.RemoveAt(convexHull.Count - 1);
                }
                convexHull.Add(p);
            }

            // 4. return the convexHull

            /**
                Has not been tested yet. Will require further work
            **/
            return convexHull;
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
        public List<(int, int)> GetBeetleMovingPositions(string piece, Dictionary<string, (int, int)> piece_positions, Dictionary<(int, int), Piece> board)
        {
            return new List<(int, int)>() {(0, 0)};
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
        public List<(int, int)> GetGrasshopperMovingPositions(string piece, Dictionary<string, (int, int)> piece_positions, Dictionary<(int, int), Piece> board)
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
        public List<(int, int)> GetQueenBeeMovingPositions(string piece, Dictionary<string, (int, int)> piece_positions, Dictionary<(int, int), Piece> board)
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
        public List<(int, int)> GetSpiderMovingPositions(string piece, Dictionary<string, (int, int)> piece_positions, Dictionary<(int, int), Piece> board)
        {
            return new List<(int, int)>() {(0, 0)};
        }

        private bool _HasOpponentNeighbor((int, int) point, Dictionary<string, (int, int)> sides, Color PlacingPieceColor, Dictionary<(int, int), Piece> board)
        {
            foreach ((int, int) neighborPosition in sides.Values)
            {
                (int, int) potentialOpponentNeighborPosition = (point.Item1 + neighborPosition.Item1, point.Item2 + neighborPosition.Item2);
                // If someone is there                                  AND is not the same color as the piece that is about to be placed
                if (board.ContainsKey(potentialOpponentNeighborPosition) && board[potentialOpponentNeighborPosition].Color != PlacingPieceColor)
                {
                    // Has an opponent neighbor
                    return true;
                }
            }

            return false;
        }

        // Has not been tested yet.
        public List<(int, int)> GetPlacingPositions(Piece movingPiece, Dictionary<(int, int), Piece> board, Dictionary<string, (int, int)> sides)
        {
            List<(int, int)> positions = new List<(int, int)>();
            // Go through each piece on the board
            foreach (KeyValuePair<(int, int), Piece> piece in board)
            {
                // Go through each of its sides
                foreach ((int, int) side in sides.Values)
                {
                    // Get this side's coordinate
                    (int, int) potentialPosition = (piece.Key.Item1 + side.Item1, piece.Key.Item2 + side.Item2);
                        // If it's Available                                    AND  DOES NOT have an opponent neighbor 
                    if (!piece.Value.Neighbors.ContainsValue(potentialPosition) && !_HasOpponentNeighbor(potentialPosition, sides, movingPiece.Color, board))
                    {
                        // It is a valid placing position, so add it to the list
                        positions.Add(potentialPosition);
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
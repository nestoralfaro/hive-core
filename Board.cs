using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static GameCore.Utils;

namespace GameCore
{
    public class Board
    {
        public readonly Dictionary<string, (int, int)> _piece_point;
        public readonly Dictionary<(int, int), Stack<Piece>> _point_stack;
        public readonly Dictionary<Color, List<Piece> > _color_pieces;

        public Board()
        {
            // Ensuring capacities so that each time an element is added
            // there is no need to dynamically allocate more memory.
            // This is an approach that should help performance
            _point_stack = new Dictionary<(int, int), Stack<Piece>>();
            _point_stack.EnsureCapacity(22);
            
            _piece_point = new Dictionary<string, (int, int)>();
            _piece_point.EnsureCapacity(22);

            _color_pieces = new Dictionary< Color, List<Piece> >()
            {
                {Color.Black, new List<Piece>()},
                {Color.White, new List<Piece>()},
            };
            _color_pieces.EnsureCapacity(2);
        }

        public void UpdateAllNeighbors()
        {
            foreach (KeyValuePair<(int, int), Stack<Piece>> stack in _point_stack)
            {
                foreach (Piece piece in stack.Value)
                {
                    PopulateNeighborsFor(piece);
                }
            }
        }
        private void PopulateNeighborsFor(Piece piece)
        {
            piece.Neighbors.Clear();
            foreach (KeyValuePair<string, (int, int)> side in piece.Sides)  
            {
                // bool IsNeighbour = (point % side == (0, 0));
                // (int, int) neighborPoint = (point.x + side.Value.Item1, point.y + side.Value.Item2);
                bool neighbourExists = _point_stack.ContainsKey(side.Value);
                if (neighbourExists)
                {
                    piece.Neighbors[side.Key] = side.Value;
                }
            }
        }

        public void AddPiece((int x, int y) point, Piece piece)
        {
            // There are pieces at this spot    AND  It is a beetle
            if (_point_stack.ContainsKey(point) && piece.Insect == Insect.Beetle)
            {
                _point_stack[point].Push(piece);
                _piece_point.Add(piece.ToString(), point);
                _color_pieces[piece.Color].Add(piece);
                UpdateAllNeighbors();
            }

            // No pieces at this spot
            else
            {
                _point_stack.Add(point, new Stack<Piece>());
                _point_stack[point].Push(piece);
                _piece_point.Add(piece.ToString(), point);
                _color_pieces[piece.Color].Add(piece);
                UpdateAllNeighbors();
            }
        }

        public void RemovePiece(Piece piece)
        {
            (int, int) piecePointToRemove = _piece_point[piece.ToString()];
            _point_stack[piecePointToRemove].Pop();

            // If this is an empty stack
            if (_point_stack[piecePointToRemove].Count == 0)
            {
                // Delete the reference, as it is now an open spot
                _point_stack.Remove(piecePointToRemove);
            }

            _piece_point.Remove(piece.ToString());
            _color_pieces[piece.Color].Remove(piece);
            UpdateAllNeighbors();
        }

        private void _DFS(ref Dictionary<(int, int), bool> visited, (int, int) piecePoint)
        {
            visited[piecePoint] = true;
            if (!_point_stack.ContainsKey(piecePoint))
            {
                return;
            }
            Piece curPiece = _point_stack[piecePoint].Peek();
            foreach ((int, int) neighbor in curPiece.Neighbors.Values)
            {
                if ((!visited.ContainsKey(neighbor) || (visited.ContainsKey(neighbor) && !visited[neighbor])) && _point_stack.ContainsKey(neighbor))
                    _DFS(ref visited, neighbor);
            }
        }

        public bool IsAllConnected()
        {
            var visited = new Dictionary<(int, int), bool>();
            (int, int) start = _point_stack.Keys.First();

            _DFS(ref visited, start);

            return visited.Count == _point_stack.Count;
        }

        // public bool _DoesNotBreakHive(Piece piece, (int x, int y) from, (int x, int y) to)
        // {
        //     RemovePiece(piece);
        //     AddPiece();
        //     return true;
        // }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static GameCore.Utils;

namespace GameCore
{
    public class Board
    {
        public readonly Dictionary<(int, int), Piece> _point_piece;
        public readonly Dictionary<string, (int, int)> _piece_point;
        public readonly Dictionary<Color, List<Piece> > _color_pieces;

        public Board()
        {
            // Ensuring capacities so that each time an element is added
            // there is no need to dynamically allocate more memory.
            // This is an approach that should help performance
            _point_piece = new Dictionary<(int, int), Piece>();
            _point_piece.EnsureCapacity(22);
            
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
            foreach (KeyValuePair<(int, int), Piece> piece in _point_piece)
            {
                PopulateNeighborsFor(piece);
            }
        }
        private void PopulateNeighborsFor(KeyValuePair<(int, int), Piece> piece)
        {
            piece.Value.Neighbors.Clear();
            foreach (KeyValuePair<string, (int, int)> side in piece.Value.Sides)  
            {
                // bool IsNeighbour = (point % side == (0, 0));
                // (int, int) neighborPoint = (point.x + side.Value.Item1, point.y + side.Value.Item2);
                bool neighbourExists = _point_piece.ContainsKey(side.Value);
                if (neighbourExists)
                {
                    piece.Value.Neighbors[side.Key] = side.Value;
                }
            }
        }

        public void AddPiece((int x, int y) point, Piece piece)
        {
            _point_piece.Add(point, piece);
            _piece_point.Add(piece.ToString(), point);
            _color_pieces[piece.Color].Add(piece);
            UpdateAllNeighbors();
        }

        public void RemovePiece(Piece piece)
        {
            (int, int) piecePointToRemove = _piece_point[piece.ToString()];
            _point_piece.Remove(piecePointToRemove);
            _piece_point.Remove(piece.ToString());
            _color_pieces[piece.Color].Remove(piece);
            UpdateAllNeighbors();
        }

        private void _DFS(ref Dictionary<(int, int), bool> visited, (int, int) piecePoint)
        {
            visited[piecePoint] = true;
            Piece curPiece = _point_piece[piecePoint];
            foreach ((int, int) neighbor in curPiece.Neighbors.Values)
            {
                if ((!visited.ContainsKey(neighbor) || (!visited.ContainsKey(neighbor) && !visited[neighbor])) && _point_piece.ContainsKey(neighbor))
                    _DFS(ref visited, neighbor);
            }
        }

        public bool IsAllConnected()
        {
            var visited = new Dictionary<(int, int), bool>();
            (int, int) start = _point_piece.Keys.First();

            _DFS(ref visited, start);

            return visited.Count == _point_piece.Count;
        }
    }
}
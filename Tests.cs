using System;
using System.IO;
using Xunit;
using static GameCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace GameCore
{
    public class Tests
    {
        readonly Board board = new();
        Player _blackPlayer = new('b');
        Player _whitePlayer = new('w');

        [Fact]
        public void NewBoardIsEmptyTest()
        {
            Assert.Empty(board.GetAllPieces());
        }

        [Fact]
        public void Game1()
        {
            _FirstBlackMove("bG1");
            _AssertSpotsForPiece("bG1", new List<(int, int)>() {(-1, 1), (-2, 0), (-1, -1), (1, -1), (2, 0), (1, 1)}, new List<(int, int)>());

            _BlackMove("bA1NWbG1");
            _AssertSpotsForPiece("bG1",
            new List<(int, int)>() {(-2, 2), (-3, 1), (-2, 0), (-1, -1), (1, -1), (2, 0), (1, 1), (0, 2)},  // placing
            new List<(int, int)>(){(-2, 2)}                                                                 // moving
            );
            _AssertSpotsForPiece("bA1",
            new List<(int, int)>() {(-2, 2), (-3, 1), (-2, 0), (-1, -1), (1, -1), (2, 0), (1, 1), (0, 2)},
            board.GetAllPieces()[board.GetPiecePoint()["bG1"]].GetAvailableSides()
            );
        }

        private void _AssertSpotsForPiece(string piece, List<(int, int)> placing, List<(int, int)> moving)
        {
            var point = board.GetPiecePoint()[piece];
            Assert.True(board.GetAllPieces().Count == board.GetPiecePoint().Count);
            Assert.True(board.GetAllPieces().Count == board.GetColorPieces()[piece[0] == 'b' ? Color.Black : Color.White].Count);
            Assert.True(board.GetAllPieces()[point].ToString() == piece);

            // If you were able to put the same piece again
            _AssertPlacingSpots(piece, placing);
            _AssertMovingSpots(piece, moving);
        }

        private void _AssertPlacingSpots(string piece, List<(int, int)> spots)
        {
            var returnedSpots = board.GetAllPieces()[board.GetPiecePoint()[piece]].GetPlacingPositions(board.GetColorPieces(), board.GetAllPieces());
            Assert.True(spots.Count == returnedSpots.Count);
            foreach (var spot in spots)
            {
                Assert.Contains(spot, returnedSpots);
            }
        }

        private void _AssertMovingSpots(string piece, List<(int, int)> spots)
        {
            var returnedSpots = board.GetAllPieces()[board.GetPiecePoint()[piece]].GetMovingPositions(board.GetAllPieces());
            Assert.True(spots.Count == returnedSpots.Count);
            foreach (var spot in spots)
            {
                Assert.Contains(spot, returnedSpots);
            }
        }

        private void _FirstBlackMove(string piece)
        {
            using (var input = new StringReader(piece))
            {
                Console.SetIn(input);
                board.MakeMove(ref _blackPlayer);
                Assert.True(board.GetAllPieces().ContainsKey((0, 0)));
                Assert.True(board.GetAllPieces()[(0, 0)].ToString() == piece);
                Assert.True(board.GetPiecePoint().ContainsKey(piece));
            }
        }

        private void _FirstWhiteMove(string piece)
        {
            using (var input = new StringReader(piece))
            {
                Console.SetIn(input);
                board.MakeMove(ref _whitePlayer);
                Assert.True(board.GetAllPieces().ContainsKey((0, 0)));
                Assert.True(board.GetAllPieces()[(0, 0)].ToString() == piece);
                Assert.True(board.GetPiecePoint().ContainsKey(piece));
            }
        }

        private void _BlackMove(string moveStr)
        {
            Move move = new Move(moveStr);
            using (var input = new StringReader(moveStr))
            {
                Console.SetIn(input);
                board.MakeMove(ref _blackPlayer);
            }
        }

        private void _WhiteMove(string moveStr)
        {
            Move move = new Move(moveStr);
            using (var input = new StringReader(moveStr))
            {
                Console.SetIn(input);
                board.MakeMove(ref _whitePlayer);
            }
        }
    }
}
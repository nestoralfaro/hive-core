using System;
using System.IO;
using Xunit;
using static GameCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace GameCore
{
    public class Tests
    {
        Logic logic = new Logic();
        Player _blackPlayer = new('b');
        Player _whitePlayer = new('w');

        [Fact]
        public void NewBoardIsEmptyTest()
        {
            Assert.Empty(logic.Board._point_piece);
            Assert.Empty(logic.Board._piece_point);
        }

        [Fact]
        public void Game1()
        {
            /////////////////////////////////////////////////// BlackMove 1 ///////////////////////////////////////////////////
            _FirstBlackMove("bG1");
            _AssertPiecePoint("bG1", (0, 0));
            _AssertSpotsForPiece("bG1", new List<(int, int)>() {(-1, 1), (-2, 0), (-1, -1), (1, -1), (2, 0), (1, 1)}, new List<(int, int)>());

            /////////////////////////////////////////////////// BlackMove 2 ///////////////////////////////////////////////////
            _BlackMove("bA1NWbG1");
            _AssertPiecePoint("bG1", (0, 0));
            _AssertPiecePoint("bA1", (-1, 1));

            var blackPlacingSpots = new List<(int, int)>(){(-2, 2), (-3, 1), (-2, 0), (-1, -1), (1, -1), (2, 0), (1, 1), (0, 2)};
            _AssertSpotsForPiece("bG1",
            blackPlacingSpots,                  // placing
            new List<(int, int)>(){(-2, 2)}     // moving
            );

            _AssertSpotsForPiece("bA1",
            blackPlacingSpots,
            logic.Board._point_piece[logic.Board._piece_point["bG1"]].SpotsAround);

            /////////////////////////////////////////////////// BlackMove 3 ///////////////////////////////////////////////////
            _BlackMove("bS1NEbA1");
            _AssertPiecePoint("bG1", (0, 0));
            _AssertPiecePoint("bA1", (-1, 1));
            _AssertPiecePoint("bS1", (1, 1));

            blackPlacingSpots = new List<(int, int)>() {(-2, 0), (-1, -1), (1, -1), (2, 0), (3, 1), (2, 2), (0, 2), (-2, 2), (-3, 1)};

            _AssertSpotsForPiece("bG1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(-2, 2), (2, 2)}     // moving
            );

            _AssertSpotsForPiece("bA1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(0, 2), (-2, 0), (-1, -1), (1, -1), (2, 0), (3, 1), (2, 2)}         // moving
            );

            _AssertSpotsForPiece("bS1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(-3, 1), (-1, -1)}     // moving
            );

            /////////////////////////////////////////////////// BlackMove 4 ///////////////////////////////////////////////////
            _BlackMove("bQ1NTbA1");
            _AssertPiecePoint("bG1", (0, 0));
            _AssertPiecePoint("bA1", (-1, 1));
            _AssertPiecePoint("bS1", (1, 1));
            _AssertPiecePoint("bQ1", (0, 2));
            blackPlacingSpots = new List<(int, int)>() {(-2, 0), (-1, -1), (1, -1), (2, 0), (3, 1), (2, 2), (1, 3), (-1, 3), (-2, 2), (-3, 1)};

            _AssertSpotsForPiece("bG1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(-2, 2), (2, 2)}     // moving
            );

            _AssertSpotsForPiece("bA1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(-2, 0), (-1, -1), (1, -1), (2, 0), (3, 1), (2, 2), (1, 3), (-1, 3), (-2, 2)}         // moving
            );

            _AssertSpotsForPiece("bS1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(-1, -1), (-1, 3)}     // moving
            );

            /////////////////////////////////////////////////// BlackMove 5 ///////////////////////////////////////////////////
            _BlackMove("bB1NTbS1");
            _AssertPiecePoint("bG1", (0, 0));
            _AssertPiecePoint("bA1", (-1, 1));
            _AssertPiecePoint("bS1", (1, 1));
            _AssertPiecePoint("bQ1", (0, 2));
            _AssertPiecePoint("bB1", (2, 2));
            blackPlacingSpots = new List<(int, int)>() {(-2, 0), (-1, -1), (1, -1), (2, 0), (3, 1), (4, 2), (3, 3), (1, 3), (-1, 3), (-2, 2) , (-3, 1)};

            _AssertSpotsForPiece("bG1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(-2, 2), (3, 3)}     // moving
            );

            _AssertSpotsForPiece("bA1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(-2, 0), (-1, -1), (1, -1), (2, 0), (3, 1), (4, 2), (3, 3), (1, 3), (-1, 3), (-2, 2)}         // moving
            );

            _AssertSpotsForPiece("bS1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(-1, -1), (3, 3)}     // moving
            );

            _AssertSpotsForPiece("bB1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(1, 1), (0, 2), (3, 1), (1, 3)}     // moving
            );

            /////////////////////////////////////////////////// BlackMove 6 ///////////////////////////////////////////////////
            _BlackMove("bG2SEbS1");
            _AssertPiecePoint("bG1", (0, 0));
            _AssertPiecePoint("bA1", (-1, 1));
            _AssertPiecePoint("bS1", (1, 1));
            _AssertPiecePoint("bQ1", (0, 2));
            _AssertPiecePoint("bB1", (2, 2));
            _AssertPiecePoint("bG2", (2, 0));
            blackPlacingSpots = new List<(int, int)>() {(-2, 0), (-1, -1), (1, -1), (3, -1), (4, 0), (3, 1), (4, 2), (3, 3), (1, 3), (-1, 3), (-2, 2), (-3, 1)};

            _AssertSpotsForPiece("bG1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(-2, 2), (3, 3), (4, 0)}     // moving
            );

            _AssertSpotsForPiece("bA1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(-2, 0), (-1, -1), (1, -1), (3, -1), (4, 0), (3, 1), (4, 2), (3, 3), (1, 3), (-1, 3), (-2, 2)}         // moving
            );

            _AssertSpotsForPiece("bS1",
            blackPlacingSpots,      // placing
            new List<(int, int)>()  // moving
            );

            _AssertSpotsForPiece("bB1",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(1, 1), (0, 2), (3, 1), (1, 3)}     // moving
            );

            _AssertSpotsForPiece("bG2",
            blackPlacingSpots,  // placing
            new List<(int, int)>(){(-1, 3), (-2, 0)}     // moving
            );

            // /////////////////////////////////////////////////// BlackMove 7 ///////////////////////////////////////////////////
            // _BlackMove("bA2NEbB1");
            // _AssertPiecePoint("bG1", (0, 0));
            // _AssertPiecePoint("bA1", (-1, 1));
            // _AssertPiecePoint("bS1", (1, 1));
            // _AssertPiecePoint("bQ1", (0, 2));
            // _AssertPiecePoint("bB1", (2, 2));
            // _AssertPiecePoint("bG2", (2, 0));
            // _AssertPiecePoint("bA2", (4, 2));
            // blackPlacingSpots = new List<(int, int)>() {(-2, 0), (-1, -1), (1, -1), (3, -1), (4, 0), (3, 1), (5, 1), (6, 2), (5, 3), (3, 3), (1, 3), (-1, 3), (-2, 2), (-3, 1)};

            // _AssertSpotsForPiece("bG1",
            // blackPlacingSpots,  // placing
            // new List<(int, int)>(){(-2, 2), (3, 3), (4, 0)}     // moving
            // );

            // _AssertSpotsForPiece("bA1",
            // blackPlacingSpots,  // placing
            // new List<(int, int)>() {(-2, 0), (-1, -1), (1, -1), (3, -1), (4, 0), (3, 1), (5, 1), (6, 2), (5, 3), (3, 3), (1, 3), (-1, 3), (-2, 2)}
            // );

            // _AssertSpotsForPiece("bS1",
            // blackPlacingSpots,      // placing
            // new List<(int, int)>()  // moving
            // );

            // _AssertSpotsForPiece("bB1",
            // blackPlacingSpots,     // placing
            // new List<(int, int)>() // moving
            // );

            // _AssertSpotsForPiece("bG2",
            // blackPlacingSpots,  // placing
            // new List<(int, int)>(){(-1, 3), (-2, 0)}     // moving
            // );

            // _AssertSpotsForPiece("bA2",
            // blackPlacingSpots,  // placing
            // new List<(int, int)>(){(3, 1), (4, 0), (3, -1), (1, -1), (-1, -1), (-2, 0), (-3, 1), (-2, 2), (-1, 3), (1, 3), (3, 3)}     // moving
            // );
        }

        private void _AssertPiecePoint(string piece, (int, int) point)
        {
            var actualPoint = logic.Board._piece_point[piece];
            Assert.True(actualPoint.Item1 == point.Item1 && actualPoint.Item2 == point.Item2);
        }

        private void _AssertSpotsForPiece(string piece, List<(int, int)> placing, List<(int, int)> moving)
        {
            var point = logic.Board._piece_point[piece];
            Assert.True(logic.Board._point_piece.Count == logic.Board._piece_point.Count);
            // Assert.True(logic.Board._point_piece.Count == logic.Board._color_pieces[piece[0] == 'b' ? Color.Black : Color.White].Count);
            Assert.True(logic.Board._point_piece[point].ToString() == piece);

            // If you were able to put the same piece again
            _AssertPlacingSpots(piece, placing);
            _AssertMovingSpots(piece, moving);
        }

        private void _AssertPlacingSpots(string piece, List<(int, int)> spots)
        {
            var returnedSpots = logic.Board._point_piece[logic.Board._piece_point[piece]].GetPlacingSpots(logic.Board);
            Assert.True(spots.Count == returnedSpots.Count);
            foreach (var spot in spots)
            {
                Assert.Contains(spot, returnedSpots);
            }
        }

        private void _AssertMovingSpots(string piece, List<(int, int)> spots)
        {
            var returnedSpots = logic.Board._point_piece[logic.Board._piece_point[piece]].GetMovingSpots(logic.Board);

            Console.WriteLine($"////////////////Actual Moving Spots For {piece}////////////////");
            foreach (var s in returnedSpots)
            {
                Console.WriteLine(s);
            }

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
                logic.MakeMove(ref _blackPlayer);
                Assert.True(logic.Board._point_piece.ContainsKey((0, 0)));
                Assert.True(logic.Board._point_piece[(0, 0)].ToString() == piece);
                Assert.True(logic.Board._piece_point.ContainsKey(piece));
            }
        }

        private void _FirstWhiteMove(string piece)
        {
            using (var input = new StringReader(piece))
            {
                Console.SetIn(input);
                logic.MakeMove(ref _whitePlayer);
                Assert.True(logic.Board._point_piece.ContainsKey((0, 0)));
                Assert.True(logic.Board._point_piece[(0, 0)].ToString() == piece);
                Assert.True(logic.Board._piece_point.ContainsKey(piece));
            }
        }

        private void _BlackMove(string moveStr)
        {
            Move move = new Move(moveStr);
            using (var input = new StringReader(moveStr))
            {
                Console.SetIn(input);
                logic.MakeMove(ref _blackPlayer);
            }
        }

        private void _WhiteMove(string moveStr)
        {
            Move move = new Move(moveStr);
            using (var input = new StringReader(moveStr))
            {
                Console.SetIn(input);
                logic.MakeMove(ref _whitePlayer);
            }
        }
    }
}
using System;
using System.IO;
using Xunit;
using static HiveCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace HiveCore
{
    public class Tests
    {
        readonly GameManager game = new();
        readonly Color _whitePlayer = Color.White;
        readonly Color _blackPlayer = Color.Black;

        [Fact]
        public void SpiderCircleTest()
        {
            _WhiteMove("wG1");
            _BlackMove("bG1NTwG1");
            _WhiteMove("wA1SEwG1");
            _BlackMove("bA1NTbG1");
            _WhiteMove("wQ1SEwA1");
            _BlackMove("bS1NEbA1");
            _WhiteMove("wS1NEwQ1"); // get it on top of wQ1
            _BlackMove("bQ1NEbS1");
            _WhiteMove("wG2NEwS1"); // get it off of wQ1
             _BlackMove("bA2SEbQ1");
            _WhiteMove("wG3NTwG2");
            _BlackMove("bB1NEbA2");
            _WhiteMove("wB1SWwG1"); // get it on top of wQ1
             _BlackMove("bB1SEbA2");
            _WhiteMove("wS1STbQ1"); // get it on top of wQ1
        }

        [Fact]
        public void AntCircleGateTest()
        {
            _WhiteMove("wG1");
            _BlackMove("bG1NTwG1");
            _WhiteMove("wA1SEwG1");
            _BlackMove("bA1NTbG1");
            _WhiteMove("wQ1SEwA1");
            _BlackMove("bS1NEbA1");
            _WhiteMove("wS1NEwQ1"); // get it on top of wQ1
            _BlackMove("bQ1NEbS1");
            _WhiteMove("wG2NEwS1"); // get it off of wQ1
            _BlackMove("bA2SEbQ1");
            _WhiteMove("wG3NTwG2");
            _BlackMove("bB1NEbA2");
            _WhiteMove("wB1SWwG1"); // get it on top of wQ1
            _BlackMove("bB1SEbA2");
            _WhiteMove("wB1STwG1"); // get it on top of wQ1
            _BlackMove("bA1NWwG2");

        }

                [Fact]

        public void SpiderCircleGateTest()
        {
            _WhiteMove("wG1");
            _BlackMove("bG1NTwG1");
            _WhiteMove("wA1SEwG1");
            _BlackMove("bS1NTbG1");
            _WhiteMove("wQ1SEwA1");
            _BlackMove("bA1NEbS1");
            _WhiteMove("wS1NEwQ1"); // get it on top of wQ1
            _BlackMove("bQ1NEbA1");
            _WhiteMove("wG2NEwS1"); // get it off of wQ1
            _BlackMove("bA2SEbQ1");
            _WhiteMove("wG3NTwG2");
            _BlackMove("bB1NEbA2");
            _WhiteMove("wB1SWwG1"); // get it on top of wQ1
            _BlackMove("bB1SEbA2");
            _WhiteMove("wB1STwG1"); // get it on top of wQ1
            _BlackMove("bS1NTbQ1");

        }
                       [Fact]

        public void BeetleGateTest()
        {
            _WhiteMove("wG1");
            _BlackMove("bG1NTwG1");
            _WhiteMove("wA1SEwG1");
            _BlackMove("bS1NTbG1");
            _WhiteMove("wQ1SEwA1");
            _BlackMove("bA1NEbS1");
            _WhiteMove("wS1NEwQ1"); // get it on top of wQ1
            _BlackMove("bQ1NEbA1");
            _WhiteMove("wG2NEwS1"); // get it off of wQ1
            _BlackMove("bA2SEbQ1");
            _WhiteMove("wG3NTwG2");
            _BlackMove("bB1NEbA2");
            _WhiteMove("wB1SWwG1"); // get it on top of wQ1
            _BlackMove("bB1SEbA2");
            _WhiteMove("wB1STwG1"); // get it on top of wQ1
            _BlackMove("bB1STbA2");

        }


        public void AntCircleTest()
        {
            _WhiteMove("wG1");
            _BlackMove("bG1NTwG1");
            _WhiteMove("wA1SEwG1");
            _BlackMove("bA1NTbG1");
            _WhiteMove("wQ1SEwA1");
            _BlackMove("bS1NEbA1");
            _WhiteMove("wS1NEwQ1"); // get it on top of wQ1
            _BlackMove("bQ1NEbS1");
            _WhiteMove("wG2NEwS1"); // get it off of wQ1
             _BlackMove("bA2SEbQ1");
            _WhiteMove("wG3NTwG2");
            _BlackMove("bB1NEbA2");
            _WhiteMove("wB1SWwG1"); // get it on top of wQ1
             _BlackMove("bB1SEbA2");
            _WhiteMove("wA1NEwG1"); // get it on top of wQ1
        }


        [Fact]
        public void NewBoardIsEmptyTest()
        {
            Assert.Empty(game.Board.Pieces);
            Assert.True(game.Board.IsEmpty());
        }

        [Fact]
        public void CantPlayTheQueenFirst()
        {
            _FirstBlackMove("bQ1");
            Assert.Empty(game.Board.Pieces);
            Assert.True(game.Board.IsEmpty());
            _FirstWhiteMove("wQ1");
            Assert.Empty(game.Board.Pieces);
            Assert.True(game.Board.IsEmpty());
        }

        // [Fact]
        // public void PlayOnlyYourPiece()
        // {
        //     Assert.Throws<ArgumentException>(() => _FirstBlackMove("wQ1"));
        //     Assert.Throws<ArgumentException>(() => _FirstWhiteMove("bQ1"));
        // }

        // [Fact]
        // public void InvalidFirstMove()
        // {
        //     Assert.Throws<ArgumentException>(() => _FirstBlackMove("bB1NWbA1"));
        //     Assert.Throws<ArgumentException>(() => _FirstWhiteMove("wQ1NEwS1"));
        // }

        [Fact]
        public void Game1()
        {
            _FirstWhiteMove("wS1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 0), (-1, -1), (1, -1), (2, 0), (1, 1)});

            _BlackMove("bG1NEwS1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 0), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (4, 0), (3, -1)});

            _WhiteMove("wQ1SWwS1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>() {(-1, 1), (-1, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-3, 1), (-4, 0), (-3, -1), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (4, 0), (3, -1)});
        }

        # region Helper Methods
        private void _BlackMove(string moveStr)
        {
            // Action move = new Action(moveStr);
            using var input = new StringReader(moveStr);
            Console.SetIn(input);
            game.HumanMove(Color.Black);
        }

        private void _WhiteMove(string moveStr)
        {
            // Action move = new Action(moveStr);
            using var input = new StringReader(moveStr);
            Console.SetIn(input);
            game.HumanMove(Color.White);
        }

        private void _AssertPiecePoint(string piece, (int, int) point)
        {
            var actualPoint = game.Board.GetPointByString(piece);
            Console.WriteLine($"Actual {piece}'s point was {actualPoint}");
            Assert.True(actualPoint.Item1 == point.Item1 && actualPoint.Item2 == point.Item2);
        }

        private void _AssertPlacingSpots(Color color, List<(int, int)> spots)
        {
            var returnedSpots = new Piece($"{char.ToLower(color.ToString()[0])}S1").GetPlacingSpots(ref game.Board);

            Console.WriteLine($"////////////////Actual Placing Spots Returned For Player {color}////////////////");
            foreach (var s in returnedSpots)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine($"Expected count: {spots.Count}");
            Console.WriteLine($"Actual count: {returnedSpots.Count}");

            Assert.True(spots.Count == returnedSpots.Count);
            foreach (var spot in spots)
            {
                Assert.Contains(spot, returnedSpots);
            }
        }

        private void _AssertMovingSpots(string piece, List<(int, int)> spots)
        {
            var returnedSpots = game.Board.GetRefTopPieceByStringName(piece)!.GetMovingSpots(ref game.Board);

            Console.WriteLine($"////////////////Actual Moving Spots For {piece}////////////////");
            foreach (var s in returnedSpots)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine($"Expected count: {spots.Count}");
            Console.WriteLine($"Actual count: {returnedSpots.Count}");

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
                if (game.HumanMove(Color.Black))
                {
                    Assert.True(game.Board.Pieces.ContainsKey((0, 0)));
                    Assert.True(game.Board.Pieces[(0, 0)].Peek().ToString() == piece);
                    Assert.True(game.Board.IsOnBoard(piece));
                }
            }
        }

        private void _FirstWhiteMove(string piece)
        {
            using (var input = new StringReader(piece))
            {
                Console.SetIn(input);
                if (game.HumanMove(Color.White))
                {
                    Assert.True(game.Board.Pieces.ContainsKey((0, 0)));
                    Assert.True(game.Board.Pieces[(0, 0)].Peek().ToString() == piece);
                    Assert.True(game.Board.IsOnBoard(piece));
                }
            }
        }

        # endregion
    }
}
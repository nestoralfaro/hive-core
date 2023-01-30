using System;
using System.IO;
using Xunit;
using static GameCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace GameCore
{
    public class Tests
    {
        readonly GameManager game = new();
        Player _blackPlayer = new(Color.Black);
        Player _whitePlayer = new(Color.White);

        [Fact]
        public void NewBoardIsEmptyTest()
        {
            Assert.Empty(game.Board.Pieces);
            Assert.True(game.Board.IsEmpty());
        }

        [Fact]
        public void CantPlayTheQueenFirst()
        {
            Assert.Throws<ArgumentException>(() => _FirstBlackMove("bQ1"));
            Assert.Throws<ArgumentException>(() => _FirstWhiteMove("bw1"));
        }

        [Fact]
        public void PlayOnlyYourPiece()
        {
            Assert.Throws<ArgumentException>(() => _FirstBlackMove("wQ1"));
            Assert.Throws<ArgumentException>(() => _FirstWhiteMove("bQ1"));
        }

        [Fact]
        public void InvalidFirstMove()
        {
            Assert.Throws<ArgumentException>(() => _FirstBlackMove("bB1NWbA1"));
            Assert.Throws<ArgumentException>(() => _FirstWhiteMove("wQ1NEwS1"));
        }

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
            _AssertMovingSpots("bG1", new List<(int, int)>() {(-4, 0)});
            _AssertMovingSpots("wQ1", new List<(int, int)>() {(-1, 1), (-1, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-3, 1), (-4, 0), (-3, -1), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (4, 0), (3, -1)});

            _BlackMove("bS1SEbG1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (3, -1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>() {(-1, 1), (-1, -1)});
            _AssertMovingSpots("bS1", new List<(int, int)>() {(1, 1), (-3, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-3, 1), (-4, 0), (-3, -1), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (4, 0), (5, -1), (4, -2), (2, -2)});

            _WhiteMove("wG1STwQ1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (3, -1));
            _AssertPiecePoint("wG1", (-3, -1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(1, 1), (-2, -2)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(-1, 1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-3, 1), (-4, 0), (-5, -1), (-4, -2), (-2 ,-2), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (4, 0), (5, -1), (4, -2), (2, -2)});

            _BlackMove("bQ1NTbS1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (3, -1));
            _AssertPiecePoint("wG1", (-3, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(5, 1), (-2, -2)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(-1, 1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>() {(3, 1), (5, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-3, 1), (-4, 0), (-5, -1), (-4, -2), (-2 ,-2), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (5, 1), (6, 0), (5, -1), (4, -2), (2, -2)});

            _WhiteMove("wG2NWwG1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (3, -1));
            _AssertPiecePoint("wG1", (-3, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (-4, 0));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(5, 1), (-2, -2)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(-1, 1), (-5, 1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>() {(3, 1), (5, -1)});
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-2, -2), (6, 0)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-3, 1), (-5, 1), (-6, 0), (-5, -1), (-4 ,-2), (-2, -2), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (5, 1), (6, 0), (5, -1), (4, -2), (2, -2)});

            // First moving of a piece on the board
            _BlackMove("bS1SEwG1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-2, -2));
            _AssertPiecePoint("wG1", (-3, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (-4, 0));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(-6, 0), (3, -1)});
            _AssertMovingSpots("wG1", new List<(int, int)>());
            _AssertMovingSpots("bQ1", new List<(int, int)>() {(3, 1), (3, -1)});
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-1, -3), (6, 0)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-3, 1), (-5, 1), (-6, 0), (-5, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (5, 1), (6, 0), (5, -1), (3, -1), (0, -2), (-1, -3), (-3, -3)});

            _WhiteMove("wG3NTwG2");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-2, -2));
            _AssertPiecePoint("wG1", (-3, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (-4, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(-6, 0), (3, -1)});
            _AssertMovingSpots("wG1", new List<(int, int)>());
            _AssertMovingSpots("bQ1", new List<(int, int)>() {(3, 1), (3, -1)});
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-1, -3), (6, 0), (-2, 2)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-5, -1), (-1, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 2), (-4, 2), (-5, 1), (-6, 0), (-5, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (5, 1), (6, 0), (5, -1), (3, -1), (0, -2), (-1, -3), (-3, -3)});

            _BlackMove("bB1NEbS1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-2, -2));
            _AssertPiecePoint("wG1", (-3, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (-4, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (0, -2));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>());
            _AssertMovingSpots("wG1", new List<(int, int)>());
            _AssertMovingSpots("bQ1", new List<(int, int)>() {(3, 1), (3, -1)});
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-2, 2), (-1, -3), (6, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-5, -1), (-1, -1)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(-1, -1), (-2, -2), (-1 ,-3)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 2), (-4, 2), (-5, 1), (-6, 0), (-5, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (5, 1), (6, 0), (5, -1), (3, -1), (2, -2), (1, -3), (-1, -3), (-3, -3)});

            _WhiteMove("wG2NEbQ1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-2, -2));
            _AssertPiecePoint("wG1", (-3, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (0, -2));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>());
            _AssertMovingSpots("wG1", new List<(int, int)>());
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-4, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-1, -1)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(-1, -1), (-2, -2), (-1 ,-3)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 2), (-4, 2), (-5, 1), (-4, 0), (-5, -1), (7, 1), (8, 0), (7, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (3, -1), (2, -2), (1, -3), (-1, -3), (-3, -3)});

            _BlackMove("bB1STwS1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-2, -2));
            _AssertPiecePoint("wG1", (-3, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(-4, 0), (3, -1)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(1, -1), (-1, -3), (-1, 1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-4, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(0, -2)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (-2, -2), (0, -2), (1, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 2), (-4, 2), (-5, 1), (-4, 0), (-5, -1), (7, 1), (8, 0), (7, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (3, -1), (0, -2), (-1, -3), (-3, -3)});

            _WhiteMove("wG1NTwQ1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-2, -2));
            _AssertPiecePoint("wG1", (-1, 1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>() {(-4, 0), (-3, -1)});
            _AssertMovingSpots("bS1", new List<(int, int)>() {(-5, 1), (3, -1)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(-3, -1), (1, -1), (-5, 1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-4, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(0, -2), (1, 1)});
            _AssertMovingSpots("bB1", new List<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(0, 2), (-2, 2), (-4, 2), (-5, 1), (-4, 0), (7, 1), (8, 0), (7, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(-4, -2), (-3, -3), (-1, -3), (0, -2), (3, -1), (3, 1)});

            _BlackMove("bS1SWwG3");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-5, 1));
            _AssertPiecePoint("wG1", (-1, 1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>() {(-4, 0), (-3, -1)});
            _AssertMovingSpots("bS1", new List<(int, int)>() {(0, 2), (-2, -2)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(-3, -1), (1, -1), (-7, 1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-4, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>());
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (1, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(0, 2), (-2, 2), (7, 1), (8, 0), (7, -1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(-6, 2), (-7, 1), (-6, 0), (-2, -2), (0, -2), (3, -1), (3, 1)});

            _WhiteMove("wG1SEwS1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-5, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertMovingSpots("wS1", new List<(int, int)>() {(-4, 2), (5, 1)});
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(-1, 1), (-2, -2)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(3, 1), (-1, 1), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-4, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>());
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (0, -2), (1, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 2), (2, -2), (7, -1), (8, 0), (7, 1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (-6, 2), (-7, 1), (-6, 0), (-2, -2)});

            _BlackMove("bG2NWbS1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-5, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-6, 2));
            _AssertMovingSpots("wS1", new List<(int, int)>() {(-4, 2), (5, 1)});
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>());
            _AssertMovingSpots("wG1", new List<(int, int)>() {(3, 1), (-1, 1), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-4, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>());
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (0, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-4, 0)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 2), (2, -2), (7, -1), (8, 0), (7, 1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(-5, 3), (-7, 3), (-8 ,2), (-7, 1), (-6, 0), (-2, -2), (3, 1)});

            _WhiteMove("wA1NTwQ1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-5, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-6, 2));
            _AssertPiecePoint("wA1", (-1, 1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>() {(-4, 0), (-3, -1)});
            _AssertMovingSpots("bS1", new List<(int, int)>());
            _AssertMovingSpots("wG1", new List<(int, int)>() {(3, 1), (-2, 2), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-4, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>());
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (0, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-4, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(-2, 2), (-4, 2), (-5, 3), (-7, 3), (-8, 2), (-7, 1), (-6, 0), (-4, 0), (-3, -1), (-2, -2), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1), (3, 1), (1, 1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(0, 2), (-2, 2), (2, -2), (7, -1), (8, 0), (7, 1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (-5, 3), (-7, 3), (-8, 2), (-7, 1), (-6, 0), (-2, -2)});

            _BlackMove("bG2SEbS1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-5, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (-1, 1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(0, 2), (-3, -1)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(3, 1), (-2, 2), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-7, 1), (-5, -1), (0, -2), (1, 1)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (0, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-2, 2), (-6, 2), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(-2, 2), (-4, 2), (-6, 2), (-7, 1), (-6, 0), (-5, -1), (-3, -1), (-2, -2), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1), (3, 1), (1, 1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(0, 2), (-2, 2), (2, -2), (7, -1), (8, 0), (7, 1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (-6, 2), (-7, 1), (-6, 0), (-5, -1), (-2, -2)});

            _WhiteMove("wA1NWbS1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-5, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (-6, 2));
            _AssertMovingSpots("wS1", new List<(int, int)>() {(5, 1), (-4, 2)});
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>());
            _AssertMovingSpots("wG1", new List<(int, int)>() {(3, 1), (-1, 1), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-7, 1), (-5, -1), (0, -2)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (0, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-2, 2), (-7, 3), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(-2, 2), (-4, 2), (-7, 1), (-6, 0), (-5, -1), (-3, -1), (-2, -2), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1), (3, 1), (1, 1), (-1, 1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 2), (-5, 3), (-7, 3), (-8, 2), (2, -2), (7, -1), (8, 0), (7, 1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (-6, 0), (-5, -1), (-2, -2)});

            _BlackMove("bA1NWbQ1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-5, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (-6, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertMovingSpots("wS1", new List<(int, int)>() {(4, 2), (-4, 2)});
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>());
            _AssertMovingSpots("wG1", new List<(int, int)>() {(4, 2), (-1, 1), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-7, 1), (-5, -1), (0, -2)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (0, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-2, 2), (-7, 3), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(-2, 2), (-4, 2), (-7, 1), (-6, 0), (-5, -1), (-3, -1), (-2, -2), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1), (4, 2), (2, 2), (1, 1), (-1, 1)});
            _AssertMovingSpots("bA1", new List<(int, int)>() {(1, 1), (-1, 1), (-2, 2), (-4, 2), (-5, 3), (-7, 3), (-8, 2), (-7, 1), (-6, 0), (-5, -1), (-3, -1), (-2, -2), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 2), (-5, 3), (-7, 3), (-8, 2), (2, -2), (7, -1), (8, 0), (7, 1)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(4, 2), (2, 2), (-6, 0), (-5, -1), (-2, -2)});

            _WhiteMove("wA1NTbA1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-5, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertMovingSpots("wS1", new List<(int, int)>() {(3, 3), (-4, 2)});
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(-1, 1), (-3, -1)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(5, 3), (-1, 1), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-7, 1), (-5, -1), (0, -2)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (0, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-2, 2), (-6, 2), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(-2, 2), (-4, 2), (-6, 2), (-7, 1), (-6, 0), (-5, -1), (-3, -1), (-2, -2), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1), (2, 2), (1, 1), (-1, 1)});
            _AssertMovingSpots("bA1", new List<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 2), (2, -2), (7, -1), (8, 0), (7, 1), (6, 2), (5, 3), (3, 3)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(-6, 0), (-5, -1), (-2, -2), (-6, 2), (-7, 1)});

            // This move is better than south because it would pin the wS1
            // This move also decreased whitePlayers placing spots, and spots available
            // do for the enemy to protect its queen
            _BlackMove("bS1NTwQ1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-1, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(3, 3), (-5, 1)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(5, 3), (-2, 2), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-5, -1), (0, -2), (1, 1)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (0, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-2, 2), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(-2, 2), (-4, 2), (-5, 1), (-6, 0), (-5, -1), (-3, -1), (-2, -2), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1), (2, 2), (1, 1), (0, 2)});
            _AssertMovingSpots("bA1", new List<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(2, -2), (7, -1), (8, 0), (7, 1), (6, 2), (5, 3), (3, 3), (-4, 2)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(-6, 0), (-5, -1), (-2, -2), (0, 2)});

            _WhiteMove("wA2NTwG2");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-1, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertPiecePoint("wA2", (7, 1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(3, 3), (-5, 1)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(5, 3), (-2, 2), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>());
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-5, -1), (0, -2), (1, 1)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (0, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-2, 2), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(2, 2), (1, 1), (0, 2), (-2, 2), (-4, 2), (-5, 1), (-6, 0), (-5, -1), (-3, -1), (-2, -2), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (9, 1), (8, 2), (6, 2), (5, 1)});
            _AssertMovingSpots("bA1", new List<(int, int)>());
            _AssertMovingSpots("wA2", new List<(int, int)>() {(5, 1), (6, 2), (5, 3), (3, 3), (2, 2), (1, 1), (0, 2), (-2, 2), (-4, 2), (-5, 1), (-6, 0), (-5, -1), (-3, -1), (-2, -2), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(5, 3), (3, 3), (-4, 2), (2, -2), (7, -1), (8, 0), (9, 1), (8, 2), (6, 2)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(0, 2), (-6, 0), (-5, -1), (-2, -2)});

            _BlackMove("bB2STbB1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-1, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertPiecePoint("wA2", (7, 1));
            _AssertPiecePoint("bB2", (-2, -2));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(3, 3), (-5, 1)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(5, 3), (-2, 2), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>());
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-5, -1), (0, -2), (1, 1)});
            _AssertMovingSpots("bB1", new List<(int, int)>());
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-2, 2), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(2, 2), (1, 1), (0, 2), (-2, 2), (-4, 2), (-5, 1), (-6, 0), (-5, -1), (-3, -1), (-4, -2), (-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (9, 1), (8, 2), (6, 2), (5, 1)});
            _AssertMovingSpots("bA1", new List<(int, int)>());
            _AssertMovingSpots("wA2", new List<(int, int)>() {(5, 1), (6, 2), (5, 3), (3, 3), (2, 2), (1, 1), (0, 2), (-2, 2), (-4, 2), (-5, 1), (-6, 0), (-5, -1), (-3, -1), (-4, -2), (-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(5, 3), (3, 3), (-4, 2), (2, -2), (7, -1), (8, 0), (9, 1), (8, 2), (6, 2)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(0, 2), (-6, 0), (-5, -1), (-4, -2), (-3, -3), (-1, -3)});

            _WhiteMove("wA2SWbB2");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-1, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertPiecePoint("wA2", (-4, -2));
            _AssertPiecePoint("bB2", (-2, -2));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(3, 3), (-5, 1)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(5, 3), (-2, 2), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-5, -1), (0, -2), (1, 1)});
            _AssertMovingSpots("bB1", new List<(int, int)>());
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-2, 2), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(2, 2), (1, 1), (0, 2), (-2, 2), (-4, 2), (-5, 1), (-6, 0), (-5, -1), (-6, -2), (-5, -3), (-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1)});
            _AssertMovingSpots("bA1", new List<(int, int)>());
            _AssertMovingSpots("wA2", new List<(int, int)>() {(-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1), (6, 2), (5, 3), (3, 3), (2, 2), (1, 1), (0, 2), (-2, 2), (-4, 2), (-5, 1), (-6, 0), (-5, -1), (-3, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(5, 3), (3, 3), (-4, 2), (-6, -2), (-5, -3), (2, -2), (7, -1), (8, 0), (7, 1), (6, 2)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(0, 2), (-6, 0), (-1, -3)});

            _BlackMove("bG3NTbS1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-1, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-3, 1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertPiecePoint("wA2", (-4, -2));
            _AssertPiecePoint("bB2", (-2, -2));
            _AssertPiecePoint("bG3", (0, 2));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>());
            _AssertMovingSpots("wG1", new List<(int, int)>() {(5, 3), (-2, 2), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-5, -1), (0, -2), (1, 1)});
            _AssertMovingSpots("bB1", new List<(int, int)>());
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-2, 2), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(2, 2), (1, 3), (-1, 3), (-2, 2), (-4, 2), (-5, 1), (-6, 0), (-5, -1), (-6, -2), (-5, -3), (-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1)});
            _AssertMovingSpots("bA1", new List<(int, int)>());
            _AssertMovingSpots("wA2", new List<(int, int)>() {(-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1), (6, 2), (5, 3), (3, 3), (2, 2), (1, 3), (-1, 3), (-2, 2), (-4, 2), (-5, 1), (-6, 0), (-5, -1), (-3, -1)});
            _AssertMovingSpots("bG3", new List<(int, int)>() {(-3, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(5, 3), (3, 3), (-4, 2), (-6, -2), (-5, -3), (2, -2), (7, -1), (8, 0), (7, 1), (6, 2)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(1, 3), (-1, 3), (-6, 0), (-1, -3)});

            _WhiteMove("wG3STbG2");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-1, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-5, -1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertPiecePoint("wA2", (-4, -2));
            _AssertPiecePoint("bB2", (-2, -2));
            _AssertPiecePoint("bG3", (0, 2));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>());
            _AssertMovingSpots("wG1", new List<(int, int)>() {(5, 3), (-2, 2), (-3, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-3, 1), (-3, -3)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (1, -1), (-2, -2)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-6, -2), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(2, 2), (1, 3), (-1, 3), (-2, 2), (-3, 1), (-5, 1), (-6, 0), (-7, -1), (-6, -2), (-5, -3), (-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1)});
            _AssertMovingSpots("bA1", new List<(int, int)>()); // should be pinned
            _AssertMovingSpots("wA2", new List<(int, int)>() {(-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1), (6, 2), (5, 3), (3, 3), (2, 2), (1, 3), (-1, 3), (-2, 2), (-3, 1), (-5, 1), (-6, 0), (-7, -1), (-6, -2)});
            _AssertMovingSpots("bG3", new List<(int, int)>() {(-3, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(5, 3), (3, 3), (-7, -1), (-6, -2), (-5, -3), (2, -2), (7, -1), (8, 0), (7, 1), (6, 2)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(1, 3), (-1, 3), (-2, 2), (-5, 1), (-1, -3)});

            _BlackMove("bG3STwQ1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-1, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-5, -1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertPiecePoint("wA2", (-4, -2));
            _AssertPiecePoint("bB2", (-2, -2));
            _AssertPiecePoint("bG3", (-3, -1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(-6, 0), (3, 3)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(5, 3), (-2, 2), (-7, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-3, 1), (-3, -3), (3, -1)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (-2, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-6, -2), (-1, -3), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(2, 2), (1, 1), (0, 2), (-2, 2), (-3, 1), (-5, 1), (-6, 0), (-7, -1), (-6, -2), (-5, -3), (-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1)});
            _AssertMovingSpots("bA1", new List<(int, int)>()); // should be pinned
            _AssertMovingSpots("wA2", new List<(int, int)>() {(-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1), (5, -1), (7, -1), (8, 0), (7, 1), (5, 1), (6, 2), (5, 3), (3, 3), (2, 2), (1, 1), (0, 2), (-2, 2), (-3, 1), (-5, 1), (-6, 0), (-7, -1), (-6, -2)});
            _AssertMovingSpots("bG3", new List<(int, int)>() {(0, 2), (-5, 1), (-7, -1), (-5, -3), (-1, -3), (3, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(5, 3), (3, 3), (-7, -1), (-6, -2), (-5, -3), (2, -2), (7, -1), (8, 0), (7, 1), (6, 2)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(0, 2), (-2, 2), (-5, 1), (-1, -3)});

            _WhiteMove("wA2STwG2");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-1, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-5, -1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertPiecePoint("wA2", (5, -1));
            _AssertPiecePoint("bB2", (-2, -2));
            _AssertPiecePoint("bG3", (-3, -1));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(-6, 0), (3, 3)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(5, 3), (-2, 2), (-7, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0), (4, -2)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-3, 1), (3, -1)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (-2, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-6, -2), (-1, -3), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(2, 2), (1, 1), (0, 2), (-2, 2), (-3, 1), (-5, 1), (-6, 0), (-7, -1), (-6, -2), (-4, -2), (-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1), (4, -2), (6, -2), (7, -1), (8, 0), (7, 1), (5, 1)});
            _AssertMovingSpots("bA1", new List<(int, int)>());
            _AssertMovingSpots("wA2", new List<(int, int)>() {(7, -1), (8, 0), (7, 1), (5, 1), (6, 2), (5, 3), (3, 3), (2, 2), (1, 1), (0, 2), (-2, 2), (-3, 1), (-5, 1), (-6, 0), (-7, -1), (-6, -2), (-4, -2), (-3, -3), (-1, -3), (0, -2), (2, -2), (3, -1)});
            _AssertMovingSpots("bG3", new List<(int, int)>() {(0, 2), (-5, 1), (-7, -1), (-1, -3), (3, -1)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(5, 3), (3, 3), (-7, -1), (-6, -2), (2, -2), (4, -2), (6, -2), (7, -1), (8, 0), (7, 1), (6, 2)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(0, 2), (-2, 2), (-5, 1), (-3, -3), (-1, -3)});

            // havent done this on the actual game
            _BlackMove("bA2STbB2");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-1, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-5, -1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertPiecePoint("wA2", (5, -1));
            _AssertPiecePoint("bB2", (-2, -2));
            _AssertPiecePoint("bG3", (-3, -1));
            _AssertPiecePoint("bA2", (-3, -3));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(-6, 0), (3, 3)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(5, 3), (-2, 2), (-7, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0), (4, -2)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-3, 1), (3, -1)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (-2, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-6, -2), (-1, -3), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(2, 2), (1, 1), (0, 2), (-2, 2), (-3, 1), (-5, 1), (-6, 0), (-7, -1), (-6, -2), (-4, -2), (-5, -3), (-4, -4), (-2, -4), (-1, -3), (0, -2), (2, -2), (3, -1), (4, -2), (6, -2), (7, -1), (8, 0), (7, 1), (5, 1)});
            _AssertMovingSpots("bA1", new List<(int, int)>());
            _AssertMovingSpots("wA2", new List<(int, int)>() {(7, -1), (8, 0), (7, 1), (5, 1), (6, 2), (5, 3), (3, 3), (2, 2), (1, 1), (0, 2), (-2, 2), (-3, 1), (-5, 1), (-6, 0), (-7, -1), (-6, -2), (-4, -2), (-5, -3), (-4, -4), (-2, -4), (-1, -3), (0, -2), (2, -2), (3, -1)});
            _AssertMovingSpots("bG3", new List<(int, int)>() {(0, 2), (-5, 1), (-7, -1), (-1, -3), (3, -1)});
            _AssertMovingSpots("bA2", new List<(int, int)>() {(-1, -3), (0, -2), (2, -2), (3, -1), (4, -2), (6, -2), (7, -1), (8, 0), (7, 1), (5, 1), (6, 2), (5, 3), (3, 3), (2, 2), (1, 1), (0, 2), (-2, 2), (-3, 1), (-5, 1), (-6, 0), (-7, -1), (-6, -2), (-4, -2)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(5, 3), (3, 3), (-7, -1), (-6, -2), (2, -2), (4, -2), (6, -2), (7, -1), (8, 0), (7, 1), (6, 2)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(0, 2), (-2, 2), (-5, 1), (-5, -3), (-4, -4), (-2, -4), (-1, -3)});

            _WhiteMove("wA3SEwA2");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-1, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-5, -1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertPiecePoint("wA2", (5, -1));
            _AssertPiecePoint("bB2", (-2, -2));
            _AssertPiecePoint("bG3", (-3, -1));
            _AssertPiecePoint("bA2", (-3, -3));
            _AssertPiecePoint("wA3", (6, -2));
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>() {(-6, 0), (3, 3)});
            _AssertMovingSpots("wG1", new List<(int, int)>() {(5, 3), (-2, 2), (-7, -1)});
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>() {(-6, 0), (4, -2)});
            _AssertMovingSpots("wG3", new List<(int, int)>() {(-3, 1), (3, -1)});
            _AssertMovingSpots("bB1", new List<(int, int)>() {(0, 0), (-2, 0), (-3, -1), (-2, -2), (1, -1)});
            _AssertMovingSpots("bG2", new List<(int, int)>() {(-6, -2), (-1, -3), (8, 0)});
            _AssertMovingSpots("wA1", new List<(int, int)>() {(2, 2), (1, 1), (0, 2), (-2, 2), (-3, 1), (-5, 1), (-6, 0), (-7, -1), (-6, -2), (-4, -2), (-5, -3), (-4, -4), (-2, -4), (-1, -3), (0, -2), (2, -2), (3, -1), (4, -2), (5, -3), (7, -3), (8, -2), (7, -1), (8, 0), (7, 1), (5, 1)});
            _AssertMovingSpots("bA1", new List<(int, int)>());
            _AssertMovingSpots("wA2", new List<(int, int)>());
            _AssertMovingSpots("bG3", new List<(int, int)>() {(0, 2), (-5, 1), (-7, -1), (-1, -3), (3, -1)});
            _AssertMovingSpots("bA2", new List<(int, int)>() {(-1, -3), (0, -2), (2, -2), (3, -1), (4, -2), (5, -3), (7, -3), (8, -2), (7, -1), (8, 0), (7, 1), (5, 1), (6, 2), (5, 3), (3, 3), (2, 2), (1, 1), (0, 2), (-2, 2), (-3, 1), (-5, 1), (-6, 0), (-7, -1), (-6, -2), (-4, -2)});
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(5, 3), (3, 3), (-7, -1), (-6, -2), (2, -2), (4, -2), (5, -3), (7, -3), (8, -2), (7, -1), (8, 0), (7, 1), (6, 2)});
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(0, 2), (-2, 2), (-5, 1), (-5, -3), (-4, -4), (-2, -4), (-1, -3)});

            // Killing spot. Black wins
            _BlackMove("bA2NWwQ1");
            _AssertPiecePoint("wS1", (0, 0));
            _AssertPiecePoint("bG1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bS1", (-1, 1));
            _AssertPiecePoint("wG1", (1, -1));
            _AssertPiecePoint("bQ1", (4, 0));
            _AssertPiecePoint("wG2", (6, 0));
            _AssertPiecePoint("wG3", (-5, -1));
            _AssertPiecePoint("bB1", (-1, -1));
            _AssertPiecePoint("bG2", (-4, 0));
            _AssertPiecePoint("wA1", (4, 2));
            _AssertPiecePoint("bA1", (3, 1));
            _AssertPiecePoint("wA2", (5, -1));
            _AssertPiecePoint("bB2", (-2, -2));
            _AssertPiecePoint("bG3", (-3, -1));
            _AssertPiecePoint("bA2", (-3, 1));
            _AssertPiecePoint("wA3", (6, -2));

            // Game is over
            _AssertMovingSpots("wS1", new List<(int, int)>());
            _AssertMovingSpots("bG1", new List<(int, int)>());
            _AssertMovingSpots("wQ1", new List<(int, int)>());
            _AssertMovingSpots("bS1", new List<(int, int)>());
            _AssertMovingSpots("wG1", new List<(int, int)>());
            _AssertMovingSpots("bQ1", new List<(int, int)>());
            _AssertMovingSpots("wG2", new List<(int, int)>());
            _AssertMovingSpots("wG3", new List<(int, int)>());
            _AssertMovingSpots("bB1", new List<(int, int)>());
            _AssertMovingSpots("bG2", new List<(int, int)>());
            _AssertMovingSpots("wA1", new List<(int, int)>());
            _AssertMovingSpots("bA1", new List<(int, int)>());
            _AssertMovingSpots("wA2", new List<(int, int)>());
            _AssertMovingSpots("bG3", new List<(int, int)>());
            _AssertMovingSpots("bA2", new List<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new List<(int, int)>());
            _AssertPlacingSpots(_blackPlayer, new List<(int, int)>());
        }
        # region Helper Methods
        private void _AssertPiecePoint(string piece, (int, int) point)
        {
            var actualPoint = game.Board.GetPointByString(piece);
            Console.WriteLine($"Actual {piece}'s point was {actualPoint}");
            Assert.True(actualPoint.Item1 == point.Item1 && actualPoint.Item2 == point.Item2);
        }

        private void _AssertPlacingSpots(Player player, List<(int, int)> spots)
        {
            var returnedSpots = player.GetPlacingSpots(ref game.Board);

            Console.WriteLine($"////////////////Actual Placing Spots Returned For Player {player.Color}////////////////");
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
            var returnedSpots = game.Board.GetTopPieceByStringName(piece).GetMovingSpots(ref game.Board);

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
                if (game.MakeMove(ref _blackPlayer))
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
                if (game.MakeMove(ref _whitePlayer))
                {
                    Assert.True(game.Board.Pieces.ContainsKey((0, 0)));
                    Assert.True(game.Board.Pieces[(0, 0)].Peek().ToString() == piece);
                    Assert.True(game.Board.IsOnBoard(piece));
                }
            }
        }

        private void _BlackMove(string moveStr)
        {
            Action move = new Action(moveStr);
            using (var input = new StringReader(moveStr))
            {
                Console.SetIn(input);
                game.MakeMove(ref _blackPlayer);
            }
        }

        private void _WhiteMove(string moveStr)
        {
            Action move = new Action(moveStr);
            using (var input = new StringReader(moveStr))
            {
                Console.SetIn(input);
                game.MakeMove(ref _whitePlayer);
            }
        }
        # endregion
    }
}
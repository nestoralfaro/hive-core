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

        public void AntRingCase()
        {
            _WhiteMove("wG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(1, 1), (-1, 1), (-2, 0), (-1, -1), (1, -1), (2, 0)});

            _BlackMove("bB1NEwG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bB1", (2, 0));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bB1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-1, 1), (-2, 0), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(3, 1), (4, 0), (3, -1)});

            _WhiteMove("wQ1SWwG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bB1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bB1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(-1, -1), (-1, 1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-1, 1), (-3, 1), (-4, 0), (-3, -1), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(3, 1), (4, 0), (3, -1)});

            _BlackMove("bQ1NTbB1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bB1", (2, 0));
            _AssertPiecePoint("wQ1", (-2, 0));
            _AssertPiecePoint("bQ1", (3, 1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bB1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(-1, -1), (-1, 1)});
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>() {(1, 1), (4, 0)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-1, 1), (-3, 1), (-4, 0), (-3, -1), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(2, 2), (4, 2), (5, 1), (4, 0), (3, -1)});

            _WhiteMove("wQ1NWwG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bB1", (2, 0));
            _AssertPiecePoint("wQ1", (-1, 1));
            _AssertPiecePoint("bQ1", (3, 1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bB1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(1, 1), (-2, 0)});
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>() {(1, 1), (4, 0)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(0, 2), (-2, 2), (-3, 1), (-2, 0), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(2, 2), (4, 2), (5, 1), (4, 0), (3, -1)});

            _BlackMove("bA1NWbQ1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bB1", (2, 0));
            _AssertPiecePoint("wQ1", (-1, 1));
            _AssertPiecePoint("bQ1", (3, 1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bB1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(1, 1), (-2, 0)});
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>() {(4, 2), (5, 1), (4, 0), (3, -1), (1, -1), (-1, -1), (-2, 0), (-3, 1), (-2, 2), (0, 2), (1, 1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 2), (-3, 1), (-2, 0), (-1, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(1, 3), (3, 3), (4, 2), (5, 1), (4, 0), (3, -1)});
        }
        [Fact]
        public void BeetleCTest(){
            //white move one
             _WhiteMove("wG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(1, 1), (-1, 1), (-2, 0), (-1, -1), (1, -1), (2, 0)});
            //black move one
            _BlackMove("bG1NTwG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (1, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (2, 2), (3, 1)});
            //white move two
            _WhiteMove("wQ1SWwG1");
            _AssertPiecePoint("wQ1", (-2,0));
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>(){(-1,-1), (-1,1)});
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>() );
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-3,1), (-4,0), (-3,-1), (-1,-1), (1,-1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)> () {(0,2), (2,2), (3,1)});
            //Black move two
            _BlackMove("bQ1NWbG1");
            _AssertPiecePoint("bQ1", (0,2));
            _AssertPiecePoint("wQ1", (-2,0));
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>(){(-1,1), (2,2)});
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>(){(-1,-1), (-1,1)});
            _AssertMovingSpots("bG1", new HashSet<(int, int)>() );
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(-2,2), (-1,3), (1,3), (2,2), (3,1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-3,1), (-4,0), (-3,-1), (-1,-1), (1,-1)});
            //White Move three
            _WhiteMove("wB1NWwQ1");
            _AssertPiecePoint("wB1", (-3,1));
            _AssertPiecePoint("bQ1", (0,2));
            _AssertPiecePoint("wQ1", (-2,0));
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertMovingSpots("wB1", new HashSet<(int, int)>() {(-2,0), (-1,1), (-4,0)});
            //At this point the beetle should not be able to move to (-2,2) but it can 
            

        }
        [Fact]
        public void SpiderCircleTest()
        {
            _WhiteMove("wG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(1, 1), (-1, 1), (-2, 0), (-1, -1), (1, -1), (2, 0)});

            _BlackMove("bG1NTwG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (1, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (2, 2), (3, 1)});

            _WhiteMove("wA1SEwG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (2, -2), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (2, 2), (3, 1)});

            _BlackMove("bA1NTbG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (2, -2), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (4, 2), (3, 1)});

            _WhiteMove("wQ1SEwA1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(0, -2), (3, -1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (4, -2), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (4, 2), (3, 1)});

            _BlackMove("bS1NEbA1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(0, -2), (3, -1)});
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (4, -2), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (6, 2), (5, 1), (3, 1)});

            _WhiteMove("wS1NEwQ1"); // get it on top of wQ1
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>() {(3, 1), (0, -2)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (6, -2), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (6, 2), (5, 1), (3, 1)});

            _BlackMove("bQ1NEbS1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>() {(3, 1), (0, -2)});
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>() {(5, 3), (5, 1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (6, -2), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (7, 1), (5, 1), (3, 1)});

            _WhiteMove("wG2NEwS1"); // get it off of wQ1
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>());
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>() {(5, 3), (5, 1)});
            _AssertMovingSpots("wG2", new HashSet<(int, int)>() {(0, -2)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (7, -1), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (7, 1), (5, 1), (3, 1)});

            _BlackMove("bA2SEbQ1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>());
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("wG2", new HashSet<(int, int)>() {(0, -2)});
            _AssertMovingSpots("bA2", new HashSet<(int, int)>() {(5, 1), (3, 1), (2, 0), (3, -1), (5, -1), (7, -1), (8, -2), (7, -3), (5, -3), (3, -3), (1, -3), (0, -2), (-1, -1), (-2, 0), (-1, 1), (0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (7, -1), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (9, 1), (8, 0), (6, 0), (5, 1), (3, 1)});

            _WhiteMove("wG3NTwG2");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertPiecePoint("wG3", (7, -1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>());
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("wG2", new HashSet<(int, int)>());
            _AssertMovingSpots("bA2", new HashSet<(int, int)>() {(5, 1), (3, 1), (2, 0), (3, -1), (5, -1), (6, 0), (8, 0), (9, -1), (8, -2), (7, -3), (5, -3), (3, -3), (1, -3), (0, -2), (-1, -1), (-2, 0), (-1, 1), (0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2)});
            _AssertMovingSpots("wG3", new HashSet<(int, int)>() {(5, -3)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (9, 1), (5, 1), (3, 1)});


            _BlackMove("bB1NEbA2");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertPiecePoint("wG3", (7, -1));
            _AssertPiecePoint("bB1", (9, 1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>());
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("wG2", new HashSet<(int, int)>());
            _AssertMovingSpots("bA2", new HashSet<(int, int)>());
            _AssertMovingSpots("wG3", new HashSet<(int, int)>() {(5, -3)});
            _AssertMovingSpots("bB1", new HashSet<(int, int)>() {(8, 2), (7, 1), (8, 0)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (10, 2), (11, 1), (10, 0), (3, 1), (5, 1)});

            _WhiteMove("wB1SWwG1"); // get it on top of wQ1
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertPiecePoint("wG3", (7, -1));
            _AssertPiecePoint("bB1", (9, 1));
            _AssertPiecePoint("wB1", (-2, 0));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>());
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("wG2", new HashSet<(int, int)>());
            _AssertMovingSpots("bA2", new HashSet<(int, int)>());
            _AssertMovingSpots("wG3", new HashSet<(int, int)>() {(5, -3)});
            _AssertMovingSpots("bB1", new HashSet<(int, int)>() {(8, 2), (7, 1), (8, 0)});
            _AssertMovingSpots("wB1", new HashSet<(int, int)>() {(-1, 1), (0, 0), (-1, -1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-3, 1), (-4, 0), (-3, -1), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (10, 2), (11, 1), (10, 0), (3, 1), (5, 1)});

            _BlackMove("bB1SEbA2");         // full circle
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertPiecePoint("wG3", (7, -1));
            _AssertPiecePoint("bB1", (8, 0));
            _AssertPiecePoint("wB1", (-2, 0));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>() {(3, 3), (-1, -1)});
            //                                                  outside ring                                                                                                                                                                     inside ring
            _AssertMovingSpots("wA1", new HashSet<(int, int)>() {(0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (10, 0), (9, 1), (8, 2), (7, 3), (5, 3), (3, 3), (1, 3), (0, 2), (-1, 1), (-3, 1), (-4, 0), (-3, -1), (-1, -1), (3, -1), (5, -1), (6, 0), (5, 1), (3, 1), (2, 0)});
            //                                                  ONLY outside ring
            _AssertMovingSpots("bA1", new HashSet<(int, int)>() {(3, 3), (5, 3), (7, 3), (8, 2), (9, 1), (10, 0), (9, -1), (8, -2), (7, -3), (5, -3), (3, -3), (1, -3), (0, -2), (-1, -1), (-3, -1), (-4, 0), (-3, 1), (-1, 1), (0, 2)});
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(0, -2), (3, -3)});
            _AssertMovingSpots("bS1", new HashSet<(int, int)>() {(0, 2), (3, -1), (5, -1), (8, 2)});    // this test case failed with improved _GetSpiderMovingSpots, but passed with previous implementation. Was missing 2 moves: (0, 2), (5, -1)
            _AssertMovingSpots("wS1", new HashSet<(int, int)>() {(0, -2), (8, -2), (5, 1), (3, 1)});    // this test case failed with improved _GetSpiderMovingSpots, but passed with previous implementation
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>() {(5, 3), (8, 2)});
            _AssertMovingSpots("wG2", new HashSet<(int, int)>() {(9, 1), (0, -2)});
            //                                                  outside ring                                                                                                                                                                    // inside ring
            _AssertMovingSpots("bA2", new HashSet<(int, int)>() {(9, 1), (10, 0), (9, -1), (8, -2), (7, -3), (5, -3), (3, -3), (1, -3), (0, -2), (-1, -1), (-3, -1), (-4, 0), (-3, 1), (-1, 1), (0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (6, 0), (5, -1), (3, -1), (2, 0), (3, 1), (5, 1)});
            _AssertMovingSpots("wG3", new HashSet<(int, int)>() {(9, 1), (5, -3)});
            _AssertMovingSpots("bB1", new HashSet<(int, int)>() {(9, 1), (7, 1), (7, -1), (9, -1)});
            _AssertMovingSpots("wB1", new HashSet<(int, int)>() {(-1, 1), (0, 0), (-1, -1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-3, 1), (-4, 0), (-3, -1), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (9, 1), (10, 0), (3, 1), (5, 1)});

            _WhiteMove("wS1STbQ1"); // get it on top of wQ1
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (5, 1));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertPiecePoint("wG3", (7, -1));
            _AssertPiecePoint("bB1", (8, 0));
            _AssertPiecePoint("wB1", (-2, 0));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(3, -1), (0, -2)});
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>() {(3, -1), (4, -2)});
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>() {(5, 3), (8, 2)});
            _AssertMovingSpots("wG2", new HashSet<(int, int)>() {(9, 1)});
            _AssertMovingSpots("bA2", new HashSet<(int, int)>());
            _AssertMovingSpots("wG3", new HashSet<(int, int)>());
            _AssertMovingSpots("bB1", new HashSet<(int, int)>());
            _AssertMovingSpots("wB1", new HashSet<(int, int)>() {(-1, 1), (0, 0), (-1, -1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-3, 1), (-4, 0), (-3, -1), (-1, -1), (0, -2), (1, -3), (3, -3), (4, -2), (5, -3), (7, -3), (8, -2), (5, -1), (3, -1), (4, 0)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (9, 1), (10, 0)});
        }

        [Fact]
        public void AntCircleGateTest()
        {
            _WhiteMove("wG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(1, 1), (-1, 1), (-2, 0), (-1, -1), (1, -1), (2, 0)});

            _BlackMove("bG1NTwG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (1, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (2, 2), (3, 1)});

            _WhiteMove("wA1SEwG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (2, -2), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (2, 2), (3, 1)});

            _BlackMove("bA1NTbG1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (2, -2), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (4, 2), (3, 1)});

            _WhiteMove("wQ1SEwA1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(0, -2), (3, -1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (4, -2), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (4, 2), (3, 1)});

            _BlackMove("bS1NEbA1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(0, -2), (3, -1)});
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (4, -2), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (6, 2), (5, 1), (3, 1)});

            _WhiteMove("wS1NEwQ1"); // get it on top of wQ1
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>() {(3, 1), (0, -2)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (6, -2), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (6, 2), (5, 1), (3, 1)});

            _BlackMove("bQ1NEbS1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>() {(3, 1), (0, -2)});
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>() {(5, 3), (5, 1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (6, -2), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (7, 1), (5, 1), (3, 1)});

            _WhiteMove("wG2NEwS1"); // get it off of wQ1
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>());
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>() {(5, 3), (5, 1)});
            _AssertMovingSpots("wG2", new HashSet<(int, int)>() {(0, -2)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (7, -1), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (7, 1), (5, 1), (3, 1)});

            _BlackMove("bA2SEbQ1");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>());
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("wG2", new HashSet<(int, int)>() {(0, -2)});
            _AssertMovingSpots("bA2", new HashSet<(int, int)>() {(8, 2), (7, 3), (5, 3), (3, 3), (1, 3), (0, 2), (-1, 1), (-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (7, -1), (5, -1), (3, -1), (2, 0), (3, 1), (5, 1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (7, -1), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (9, 1), (8, 0), (6, 0), (5, 1), (3, 1)});

            _WhiteMove("wG3NTwG2");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertPiecePoint("wG3", (7, -1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>());
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("wG2", new HashSet<(int, int)>());
            _AssertMovingSpots("bA2", new HashSet<(int, int)>() {(8, 2), (7, 3), (5, 3), (3, 3), (1, 3), (0, 2), (-1, 1), (-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (8, 0), (5, 1), (3, 1), (2, 0), (3, -1), (5, -1), (6, 0)});
            _AssertMovingSpots("wG3", new HashSet<(int, int)>() {(5, -3)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (9, 1), (5, 1), (3, 1)});

            _BlackMove("bB1NEbA2");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertPiecePoint("wG3", (7, -1));
            _AssertPiecePoint("bB1", (9, 1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>());
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("wG2", new HashSet<(int, int)>());
            _AssertMovingSpots("bA2", new HashSet<(int, int)>());
            _AssertMovingSpots("wG3", new HashSet<(int, int)>() {(5, -3)});
            _AssertMovingSpots("bB1", new HashSet<(int, int)>() {(8, 2), (7, 1), (8, 0)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (10, 2), (11, 1), (10, 0), (5, 1), (3, 1)});

            _WhiteMove("wB1SWwG1"); // get it on top of wQ1
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertPiecePoint("wG3", (7, -1));
            _AssertPiecePoint("bB1", (9, 1));
            _AssertPiecePoint("wB1", (-2, 0));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>());
            _AssertMovingSpots("wA1", new HashSet<(int, int)>());
            _AssertMovingSpots("bA1", new HashSet<(int, int)>());
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("bS1", new HashSet<(int, int)>());
            _AssertMovingSpots("wS1", new HashSet<(int, int)>());
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>());
            _AssertMovingSpots("wG2", new HashSet<(int, int)>());
            _AssertMovingSpots("bA2", new HashSet<(int, int)>());
            _AssertMovingSpots("wG3", new HashSet<(int, int)>() {(5, -3)});
            _AssertMovingSpots("bB1", new HashSet<(int, int)>() {(8, 2), (7, 1), (8, 0)});
            _AssertMovingSpots("wB1", new HashSet<(int, int)>() {(-1, 1), (0, 0), (-1, -1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-3, 1), (-4, 0), (-3, -1), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (10, 2), (11, 1), (10, 0), (5, 1), (3, 1)});

            _BlackMove("bB1SEbA2"); // complete circle
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertPiecePoint("wG3", (7, -1));
            _AssertPiecePoint("bB1", (8, 0));
            _AssertPiecePoint("wB1", (-2, 0));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>());
            _AssertMovingSpots("bG1", new HashSet<(int, int)>() {(3, 3), (-1, -1)});
            //                                                  inner circle                                    // outter circle
            _AssertMovingSpots("wA1", new HashSet<(int, int)>() {(2, 0), (3, 1), (5, 1), (6, 0), (5, -1), (3, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (10, 0), (9, 1), (8, 2), (7, 3), (5, 3), (3, 3), (1, 3), (0, 2), (-1, 1), (-3, 1), (-4, 0), (-3, -1), (-1, -1)});
            //                                                  only the outter circle
            _AssertMovingSpots("bA1", new HashSet<(int, int)>() {(0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (10, 0), (9, 1), (8, 2), (7, 3), (5, 3), (3, 3), (0, 2), (-1, 1), (-3, 1), (-4, 0), (-3, -1), (-1, -1)});
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(0, -2), (3, -3)});
            _AssertMovingSpots("bS1", new HashSet<(int, int)>() {(8, 2), (0, 2), (3, -1), (5, -1)});
            _AssertMovingSpots("wS1", new HashSet<(int, int)>() {(5, 1), (3, 1), (8, -2), (0, -2)});
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>() {(5, 3), (8, 2)});
            _AssertMovingSpots("wG2", new HashSet<(int, int)>() {(9, 1), (0, -2)});
            _AssertMovingSpots("bA2", new HashSet<(int, int)>() {(2, 0), (3, 1), (5, 1), (6, 0), (5, -1), (3, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (10, 0), (9, 1), (8, 2), (7, 3), (5, 3), (3, 3), (1, 3), (0, 2), (-1, 1), (-3, 1), (-4, 0), (-3, -1), (-1, -1)});
            _AssertMovingSpots("wG3", new HashSet<(int, int)>() {(9, 1), (5, -3)});
            _AssertMovingSpots("bB1", new HashSet<(int, int)>() {(9, 1), (7, 1), (7, -1), (9, -1)});
            _AssertMovingSpots("wB1", new HashSet<(int, int)>() {(-1, 1), (0, 0), (-1, -1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-3, 1), (-4, 0), (-3, -1), (-1, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (9, 1), (10, 0), (5, 1), (3, 1)});

            _WhiteMove("wB1STwG1"); // get it on top of wQ1
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertPiecePoint("wG3", (7, -1));
            _AssertPiecePoint("bB1", (8, 0));
            _AssertPiecePoint("wB1", (-1, -1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>() {(3, 3), (-2, -2), (3, -3)});
            _AssertMovingSpots("bG1", new HashSet<(int, int)>() {(3, 3), (-2, -2)});
            //                                                  inner circle
            _AssertMovingSpots("wA1", new HashSet<(int, int)>() {(2, 0), (3, 1), (5, 1), (6, 0), (5, -1), (3, -1)});
            //                                                  only the outter circle
            _AssertMovingSpots("bA1", new HashSet<(int, int)>() {(0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (10, 0), (9, 1), (8, 2), (7, 3), (5, 3), (3, 3), (0, 2), (-1, 1), (-2, 0), (-3, -1), (-2, -2)});
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(0, -2), (3, -3)});
            _AssertMovingSpots("bS1", new HashSet<(int, int)>() {(8, 2), (0, 2), (3, -1), (5, -1)});
            _AssertMovingSpots("wS1", new HashSet<(int, int)>() {(5, 1), (3, 1), (8, -2), (0, -2)});
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>() {(5, 3), (8, 2)});
            _AssertMovingSpots("wG2", new HashSet<(int, int)>() {(9, 1), (0, -2)});
            //                                                  inner circle                                    // outter circle
            _AssertMovingSpots("bA2", new HashSet<(int, int)>() {(2, 0), (3, 1), (5, 1), (6, 0), (5, -1), (3, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (10, 0), (9, 1), (8, 2), (7, 3), (5, 3), (3, 3), (1, 3), (0, 2), (-1, 1), (-2, 0), (-3, -1), (-2, -2)});
            _AssertMovingSpots("wG3", new HashSet<(int, int)>() {(9, 1), (5, -3)});
            _AssertMovingSpots("bB1", new HashSet<(int, int)>() {(9, 1), (7, 1), (7, -1), (9, -1)});
            _AssertMovingSpots("wB1", new HashSet<(int, int)>() {(0, 0), (-2, 0), (0, -2), (1, -1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-3, -1), (-2, -2), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (9, 1), (10, 0), (5, 1), (3, 1)});

            // this would be an invalid move
            // meaning that the rest should stay the same as the previous set
            _BlackMove("bA1NWwG2");
            _AssertPiecePoint("wG1", (0, 0));
            _AssertPiecePoint("bG1", (1, 1));
            _AssertPiecePoint("wA1", (1, -1));
            _AssertPiecePoint("bA1", (2, 2));
            _AssertPiecePoint("wQ1", (2, -2));
            _AssertPiecePoint("bS1", (4, 2));
            _AssertPiecePoint("wS1", (4, -2));
            _AssertPiecePoint("bQ1", (6, 2));
            _AssertPiecePoint("wG2", (6, -2));
            _AssertPiecePoint("bA2", (7, 1));
            _AssertPiecePoint("wG3", (7, -1));
            _AssertPiecePoint("bB1", (8, 0));
            _AssertPiecePoint("wB1", (-1, -1));
            _AssertMovingSpots("wG1", new HashSet<(int, int)>() {(3, 3), (-2, -2), (3, -3)});
            _AssertMovingSpots("bG1", new HashSet<(int, int)>() {(3, 3), (-2, -2)});
            //                                                  inner circle
            _AssertMovingSpots("wA1", new HashSet<(int, int)>() {(2, 0), (3, 1), (5, 1), (6, 0), (5, -1), (3, -1)});
            //                                                  only the outter circle
            _AssertMovingSpots("bA1", new HashSet<(int, int)>() {(0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (10, 0), (9, 1), (8, 2), (7, 3), (5, 3), (3, 3), (0, 2), (-1, 1), (-2, 0), (-3, -1), (-2, -2)});
            _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(0, -2), (3, -3)});
            _AssertMovingSpots("bS1", new HashSet<(int, int)>() {(8, 2), (0, 2), (3, -1), (5, -1)});
            _AssertMovingSpots("wS1", new HashSet<(int, int)>() {(5, 1), (3, 1), (8, -2), (0, -2)});
            _AssertMovingSpots("bQ1", new HashSet<(int, int)>() {(5, 3), (8, 2)});
            _AssertMovingSpots("wG2", new HashSet<(int, int)>() {(9, 1), (0, -2)});
            //                                                  inner circle                                    // outter circle
            _AssertMovingSpots("bA2", new HashSet<(int, int)>() {(2, 0), (3, 1), (5, 1), (6, 0), (5, -1), (3, -1), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (9, -1), (10, 0), (9, 1), (8, 2), (7, 3), (5, 3), (3, 3), (1, 3), (0, 2), (-1, 1), (-2, 0), (-3, -1), (-2, -2)});
            _AssertMovingSpots("wG3", new HashSet<(int, int)>() {(9, 1), (5, -3)});
            _AssertMovingSpots("bB1", new HashSet<(int, int)>() {(9, 1), (7, 1), (7, -1), (9, -1)});
            _AssertMovingSpots("wB1", new HashSet<(int, int)>() {(0, 0), (-2, 0), (0, -2), (1, -1)});
            _AssertPlacingSpots(_whitePlayer, new HashSet<(int, int)>() {(-2, 0), (-3, -1), (-2, -2), (0, -2), (1, -3), (3, -3), (5, -3), (7, -3), (8, -2), (5, -1), (3, -1)});
            _AssertPlacingSpots(_blackPlayer, new HashSet<(int, int)>() {(0, 2), (1, 3), (3, 3), (5, 3), (7, 3), (8, 2), (9, 1), (10, 0), (5, 1), (3, 1)});
        }

        //         [Fact]
        // public void SpiderCircleGateTest()
        // {
        //     _WhiteMove("wG1");
        //     _BlackMove("bG1NTwG1");
        //     _WhiteMove("wA1SEwG1");
        //     _BlackMove("bS1NTbG1");
        //     _WhiteMove("wQ1SEwA1");
        //     _BlackMove("bA1NEbS1");
        //     _WhiteMove("wS1NEwQ1"); // get it on top of wQ1
        //     _BlackMove("bQ1NEbA1");
        //     _WhiteMove("wG2NEwS1"); // get it off of wQ1
        //     _BlackMove("bA2SEbQ1");
        //     _WhiteMove("wG3NTwG2");
        //     _BlackMove("bB1NEbA2");
        //     _WhiteMove("wB1SWwG1"); // get it on top of wQ1
        //     _BlackMove("bB1SEbA2");
        //     _WhiteMove("wB1STwG1"); // get it on top of wQ1
        //     _BlackMove("bS1NTbQ1");

        // }

        // [Fact]
        // public void AntBiggerCTest()
        // {
        //     _WhiteMove("wG1");
        //     _BlackMove("bG1NEwG1");
        //     _WhiteMove("wQ1STwG1");
        //     _BlackMove("bQ1NEbG1");
        //     _WhiteMove("wB1STwQ1");
        //     _BlackMove("bB1SEbQ1");
        //     _WhiteMove("wA1SEwB1"); // get it on top of wQ1
        //     _BlackMove("bG2SEbB1");
        //     _WhiteMove("wG2SEwA1"); // get it off of wQ1
        //     _BlackMove("bA1STbG2");
        //     _WhiteMove("wS1NEwG2");
        //     _BlackMove("bA1NEwS1");


        // }
        //                [Fact]

        // public void BeetleGateTest()
        // {
        //     _WhiteMove("wG1");
        //     _BlackMove("bG1NTwG1");
        //     _WhiteMove("wA1SEwG1");
        //     _BlackMove("bS1NTbG1");
        //     _WhiteMove("wQ1SEwA1");
        //     _BlackMove("bA1NEbS1");
        //     _WhiteMove("wS1NEwQ1"); // get it on top of wQ1
        //     _BlackMove("bQ1NEbA1");
        //     _WhiteMove("wG2NEwS1"); // get it off of wQ1
        //     _BlackMove("bA2SEbQ1");
        //     _WhiteMove("wG3NTwG2");
        //     _BlackMove("bB1NEbA2");
        //     _WhiteMove("wB1SWwG1"); // get it on top of wQ1
        //     _BlackMove("bB1SEbA2");
        //     _WhiteMove("wB1STwG1"); // get it on top of wQ1
        //     _BlackMove("bB1STbA2");

        // }

        //                        [Fact]

        // public void AntBigCTest()
        // {
        //     _WhiteMove("wG1");
        //     _BlackMove("bG1SEwG1");
        //     _WhiteMove("wA1SWwG1");
        //     _BlackMove("bS1SEbG1");
        //     _WhiteMove("wQ1STwA1");
        //     _BlackMove("bA1STbS1");
        //     _WhiteMove("wA2STwQ1"); // get it on top of wQ1
        //     _BlackMove("bQ1SWbA1");
        //     _WhiteMove("wA2SWbQ1"); // get it off of wQ1
           

        // }

        //                [Fact]

        // public void AntCTest(){
        //     _WhiteMove("wG1");
        //     _BlackMove("bB1NEwG1");
        //     _WhiteMove("wQ1STwG1");
        //     _BlackMove("bQ1NTbB1");
        //     _WhiteMove("wQ1SWwG1");
        //     _BlackMove("bA1NWbQ1");
        //     _WhiteMove("wQ1NWwG1");
        //     _BlackMove("bA1NTwQ1");
        // }



        // public void AntCircleTest()
        // {
        //     _WhiteMove("wG1");
        //     _BlackMove("bG1NTwG1");
        //     _WhiteMove("wA1SEwG1");
        //     _BlackMove("bA1NTbG1");
        //     _WhiteMove("wQ1SEwA1");
        //     _BlackMove("bS1NEbA1");
        //     _WhiteMove("wS1NEwQ1"); // get it on top of wQ1
        //     _BlackMove("bQ1NEbS1");
        //     _WhiteMove("wG2NEwS1"); // get it off of wQ1
        //      _BlackMove("bA2SEbQ1");
        //     _WhiteMove("wG3NTwG2");
        //     _BlackMove("bB1NEbA2");
        //     _WhiteMove("wB1SWwG1"); // get it on top of wQ1
        //      _BlackMove("bB1SEbA2");
        //     _WhiteMove("wA1NEwG1"); // get it on top of wQ1
        // }


        // [Fact]
        // public void NewBoardIsEmptyTest()
        // {
        //     Assert.Empty(game.Board.Pieces);
        //     Assert.True(game.Board.IsEmpty());
        // }

        // [Fact]
        // public void CantPlayTheQueenFirst()
        // {
        //     _FirstBlackMove("bQ1");
        //     Assert.Empty(game.Board.Pieces);
        //     Assert.True(game.Board.IsEmpty());
        //     _FirstWhiteMove("wQ1");
        //     Assert.Empty(game.Board.Pieces);
        //     Assert.True(game.Board.IsEmpty());
        // }

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

        // [Fact]
        // public void Game1()
        // {
        //     _FirstWhiteMove("wS1");
        //     _AssertPiecePoint("wS1", (0, 0));
        //     _AssertMovingSpots("wS1", new HashSet<(int, int)>());
        //     _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 0), (-1, -1), (1, -1), (2, 0), (1, 1)});

        //     _BlackMove("bG1NEwS1");
        //     _AssertPiecePoint("wS1", (0, 0));
        //     _AssertPiecePoint("bG1", (2, 0));
        //     _AssertMovingSpots("wS1", new HashSet<(int, int)>());
        //     _AssertMovingSpots("bG1", new HashSet<(int, int)>());
        //     _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-2, 0), (-1, -1)});
        //     _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (4, 0), (3, -1)});

        //     _WhiteMove("wQ1SWwS1");
        //     _AssertPiecePoint("wS1", (0, 0));
        //     _AssertPiecePoint("bG1", (2, 0));
        //     _AssertPiecePoint("wQ1", (-2, 0));
        //     _AssertMovingSpots("wS1", new HashSet<(int, int)>());
        //     _AssertMovingSpots("bG1", new HashSet<(int, int)>());
        //     _AssertMovingSpots("wQ1", new HashSet<(int, int)>() {(-1, 1), (-1, -1)});
        //     _AssertPlacingSpots(_whitePlayer, new List<(int, int)>() {(-1, 1), (-3, 1), (-4, 0), (-3, -1), (-1, -1)});
        //     _AssertPlacingSpots(_blackPlayer, new List<(int, int)>() {(3, 1), (4, 0), (3, -1)});
        // }

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
            Assert.True(actualPoint.x == point.Item1 && actualPoint.y == point.Item2);
        }

        private void _AssertPlacingSpots(Color color, HashSet<(int, int)> spots)
        {
            var returnedSpots = game.Board.GetPlacingSpotsFor(color);
            Console.WriteLine($"////////////////Actual Placing Spots Returned For Player {color}////////////////");
            foreach (var s in returnedSpots)
            {
                Console.WriteLine(s);
            }

            var diff = returnedSpots.Where(r => !spots.Any(a => r.Item1 == a.Item1 && r.Item2 == a.Item2)).ToList();
            diff.AddRange(spots.Where(r => !returnedSpots.Any(a => r.Item1 == a.Item1 && r.Item2 == a.Item2)).ToList());
            if (diff.Count > 0)
            {
                var output = "The difference was: ";
                foreach (var d in diff)
                {
                    output += d;
                }
                _PrintWarning(output);
            }

            Console.WriteLine($"Expected count: {spots.Count}");
            Console.WriteLine($"Actual count: {returnedSpots.Count}");

            Assert.True(spots.Count == returnedSpots.Count);
            foreach (var spot in spots)
            {
                Assert.Contains(spot, returnedSpots);
            }
        }

        private void _AssertMovingSpots(string piece, HashSet<(int, int)> spots)
        {
            Piece p = game.Board.GetRefTopPieceByStringName(piece)!;
            var returnedSpots = game.Board.GetMovingSpotsFor(ref p);

            Console.WriteLine($"////////////////Actual Moving Spots For {piece}////////////////");
            foreach (var s in returnedSpots)
            {
                Console.WriteLine(s);
            }

            var diff = returnedSpots.Where(r => !spots.Any(a => r.Item1 == a.Item1 && r.Item2 == a.Item2)).ToList();
            diff.AddRange(spots.Where(r => !returnedSpots.Any(a => r.Item1 == a.Item1 && r.Item2 == a.Item2)).ToList());
            if (diff.Count > 0)
            {
                var output = "The difference was: ";
                foreach (var d in diff)
                {
                    output += d;
                }
                _PrintWarning(output);
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
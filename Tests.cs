using System;
using System.IO;
using Xunit;

namespace GameCore
{
    public class Tests
    {

        Board board = new Board();
        Player blackPlayer = new Player('b');
        Player whitePlayer = new Player('w');

        [Fact]
        public void NewBoardIsEmptyTest()
        {
            Assert.Empty(board.GetAllPieces());
        }

        [Fact]
        public void AddingFirstBlackAnt()
        {
            string piece = "bA1";
            using (var input = new StringReader(piece))
            {
                Console.SetIn(input);
                board.MakeMove(ref blackPlayer);
                Assert.True(board.GetAllPieces().ContainsKey((0, 0)));
                Assert.True(board.GetAllPieces()[(0, 0)].ToString() == piece);
            }
        }

        // [Fact]
        // public void TestInputOutput()
        // {
        //     // Redirect the standard input and output streams
        //     using (var input = new StringReader("input"))
        //     using (var output = new StringWriter())
        //     {
        //         Console.SetIn(input);
        //         Console.SetOut(output);

        //         // Run the code that takes input from the console
        //         YourCode.Main();

        //         // Assert the output is as expected
        //         Assert.Equal("expected output", output.ToString());
        //     }
        // }

        // [Fact]
        // public void RemovingPieceFromBoardTest()
        // {
        //     var board = new Board();
        //     var piece = new Piece("Beetle", "White", 1);
        //     board.AddPiece(piece, (0, 0));
        //     board.RemovePiece((0, 0));
        //     Assert.DoesNotContain(piece, board.GetAllPieces());
        // }
    }
}
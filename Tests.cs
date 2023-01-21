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
        public void AddingbA1()
        {
            string piece = "bA1";
            using (var input = new StringReader(piece))
            {
                Console.SetIn(input);
                board.MakeMove(ref blackPlayer);
                Assert.True(board.GetAllPieces().ContainsKey((0, 0)));
                Assert.True(board.GetAllPieces()[(0, 0)].ToString() == piece);
                Assert.True(board.GetPiecePoint().ContainsKey(piece));
            }
        }

        private bool _HasOpponentNeighbor((int, int) point, Color playingColor)
        {
            foreach ((int, int) side in board._sides_offset.Values)
            {
                (int, int) potentialOpponentNeighborPosition = (point.Item1 + side.Item1, point.Item2 + side.Item2);
                // If piece is on the board                             AND is not the same color as the piece that is about to be placed
                if (board.GetAllPieces().ContainsKey(potentialOpponentNeighborPosition) && board.GetAllPieces()[potentialOpponentNeighborPosition].Color != playingColor)
                {
                    // Has an opponent neighbor
                    return true;
                }
            }

            // Checked each side, and no opponent's pieces were found
            return false;
        }

        private void _AssertPiece(string pieceStr)
        {
            var movingPiecePoint = board.GetPiecePoint()[pieceStr];
            Piece piece = board.GetAllPieces()[movingPiecePoint];
            Color color = pieceStr[0] == 'b' ? Color.Black : Color.White;

            // Check the hashmaps
            Assert.True(board.GetAllPieces()[movingPiecePoint].ToString() == pieceStr);
            Assert.True(board.GetColorPieces()[color].Contains(piece));

            // Evaluate Sides – i.e., make sure it is a valid side according to the offset
            foreach (var side in piece.Sides)
            {
                Assert.True(
                    // point % side_offset == 0
                    (side.Value.Item1 % board._sides_offset[side.Key].Item1) == 0
                    && (side.Value.Item2 % board._sides_offset[side.Key].Item2) == 0
                );
            }

            // Evaluate Neighbors – i.e., make sure neighbors do exist on the board
            foreach (var neighbor in piece.Neighbors)
            {
                Assert.True(board.GetAllPieces().ContainsKey(neighbor.Value));
            }

            // Evaluate Available Sides – i.e., each side should map to a piece on the board 
            foreach (var avalSide in piece.GetAvailableSides())
            {
                Assert.False(board.GetAllPieces().ContainsKey(avalSide));
            }

            // Had this piece not been played, what would its available spots be? 
            foreach (var spot in piece.GetPlacingPositions(board.GetColorPieces(), board.GetAllPieces(), board.GetPiecePoint().Values))
            {
                // Make there is no adjacent opponent 
                foreach (var offset in board._sides_offset.Values)
                {
                    var side = (spot.Item1 + offset.Item1, spot.Item2 + offset.Item2);
                    Assert.False(_HasOpponentNeighbor(side, color));
                }
            }

            // // Had this piece been moved, what would its available spots be?
            // foreach (var spot in piece.GetMovingPositions())
            // {
            //     Assert.False(BreaksTheHive(spot));
            //     Assert.False(IsGate(spot));
            // }
        }

        private void _BlackMove(string moveStr)
        {
            Move move = new Move(moveStr);
            using (var input = new StringReader(moveStr))
            {
                Console.SetIn(input);
                board.MakeMove(ref blackPlayer);
                _AssertPiece(move.MovingPiece);
                _AssertPiece(move.DestinationPiece);
            }
        }

        private void _WhiteMove(string moveStr)
        {
            Move move = new Move(moveStr);
            using (var input = new StringReader(moveStr))
            {
                Console.SetIn(input);
                board.MakeMove(ref whitePlayer);
                _AssertPiece(move.MovingPiece);
                _AssertPiece(move.DestinationPiece);
            }
        }

        [Fact]
        public void TestingMoves()
        {
            _BlackMove("bA2*|bA1");
        }
    }
}
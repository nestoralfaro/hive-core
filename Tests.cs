using System;
using System.IO;
using Xunit;
using static GameCore.Utils;

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
        public void PlacingOnABusySpot()
        {
            _FirstMove("bA1");
            _BlackMove("bA2*|bA1");
            int piecesOnTheBoard = board.GetAllPieces().Count;
            try
            {
                _BlackMove("bA3|*bA2");
                Assert.True(false, "Expected exception was not thrown.");
            }
            catch (Exception)
            {
                Assert.True(true);
            }

            // Since it threw the exception, it should have not added it
            Assert.Equal(piecesOnTheBoard, board.GetAllPieces().Count);
            _BlackMove("bB1*|bA2");
        }

        #region Helper Methods
        private bool _IsAntMovingSpot((int x, int y) ant, (int x, int y) spot)
        {
            return true;
        }
        private bool _IsBeetleMovingSpot((int x, int y) beetle, (int x, int y) spot)
        {
            return true;
        }
        private bool _IsGrasshopperMovingSpot((int x, int y) grasshopper,(int x, int y) spot)
        {
            return true;
        }
        private bool _IsSpiderMovingSpot((int x, int y) spider, (int x, int y) spot)
        {
            return true;
        }
        private bool _IsQueenBeeMovingSpot((int x, int y) queenBee, (int x, int y) spot)
        {
            return true;
        }

        private bool HasOneNeighborOnly((int x, int y) spot)
        {
            int neighborCount = 0;
            foreach (var side in SIDE_OFFSETS)
            {
                if (board.GetAllPieces().ContainsKey(spot))
                    ++neighborCount;
                    if (neighborCount > 1)
                        return false;
            }
            return true;
        }

        private bool _BreaksTheHive((int x, int y) spot)
        {
            // If it does not even have one neighbor, then that'd break the hive
            return !HasOneNeighborOnly(spot);
        }

        private bool _IsGate((int x, int y) spot)
        {
            return HasOneNeighborOnly(spot);
        }

        private void _FirstMove(string piece)
        {
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
            foreach ((int, int) side in SIDE_OFFSETS.Values)
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
                (int x, int y) point1 = (side.Value.Item1, side.Value.Item2); 
                (int x, int y) point2 = (SIDE_OFFSETS[side.Key].Item1, SIDE_OFFSETS[side.Key].Item1); 
                Assert.True(
                    // point % side_offset == 0
                    // (side.Value.Item1 % board._sides_offset[side.Key].Item1) == 0
                    // && (side.Value.Item2 % board._sides_offset[side.Key].Item2) == 0
                    (point2.x > 0 ? point1.x % point2.x : 0) == 0
                    && (point2.y > 0 ? point1.y % point2.y : 0) == 0
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
            foreach (var spot in piece.GetPlacingPositions(board.GetColorPieces(), board.GetAllPieces()))
            {
                // Make there is no adjacent opponent 
                foreach (var offset in SIDE_OFFSETS.Values)
                {
                    var side = (spot.Item1 + offset.Item1, spot.Item2 + offset.Item2);
                    Assert.False(_HasOpponentNeighbor(side, color));
                }
            }

            // Had this piece been moved, what would its available spots be?
            foreach (var spot in piece.GetMovingPositions(board.GetAllPieces()))
            {
                switch (piece.Insect)
                {
                    case Insect.Ant:
                        Assert.True(_IsAntMovingSpot(piece.Point, spot));
                        break;
                    case Insect.Beetle:
                        Assert.True(_IsBeetleMovingSpot(piece.Point, spot));
                        break;
                    case Insect.Grasshopper:
                        Assert.True(_IsGrasshopperMovingSpot(piece.Point ,spot));
                        break;
                    case Insect.Spider:
                        Assert.True(_IsSpiderMovingSpot(piece.Point, spot));
                        break;
                    case Insect.QueenBee:
                        Assert.True(_IsQueenBeeMovingSpot(piece.Point, spot));
                        break;
                }
                Assert.False(_BreaksTheHive(spot));
                Assert.False(_IsGate(spot));
            }
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
        #endregion
    }
}
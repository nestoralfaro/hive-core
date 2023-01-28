namespace GameCore
{
    public class AI : Player
    {
        public AI(Color color) : base(color) {}

        public void MakeMove(Board board)
        {
            // Method 1
            List<(int, int)> whiteMoves = new ();
            List<(int, int)> blackMoves = new ();

            foreach (var pieces in board.pieces)
            {
                var piece = pieces.Value.Peek();
                if (piece.Color == Color.White)
                {
                    whiteMoves.AddRange(piece.GetMovingSpots(board));
                }
                else
                {
                    blackMoves.AddRange(piece.GetMovingSpots(board));
                }
            }

            // Method 2
            // foreach (Piece piece in board._color_pieces[Color.White])
            // {
            //     List<(int, int)> moves  = piece.GetMovingSpots(board);
            //     foreach (var move in moves)
            //     {
            //         // Move such existing piece
            //         // remove piece
            //         board._RemovePiece(piece);
            //         // re-add it
            //         // piece.Point = to;
            //         // board.AddPiece(to, piece);
            //     }
            // }

            // Parallel.ForEach()

            // add a MovePiece and PlacePiece method to the board
        }
    }
}
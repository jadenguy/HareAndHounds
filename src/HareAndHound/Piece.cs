namespace HareAndHound
{
    public class Piece
    {
        public PieceType pieceType;
        public enum PieceType
        {
            Empty, Hound, Hare
        }
        private Piece() { }
        public Piece(PieceType type)
        {
            this.pieceType = type;
        }
    }
}
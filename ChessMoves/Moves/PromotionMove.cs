using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame.Moves
{
    public class PromotionMove : Move
    {
        public override Position StartPos { get; }
        public override Position EndPos { get; }
        public PieceTypes PromotionType { get; }

        public PromotionMove(Position start, Position end, PieceTypes promotionType)
        {
            StartPos = start;
            EndPos = end;
            PromotionType = promotionType;
        }

        public override void DoMove(Board board)
        {
            Piece pawn = board[StartPos];

            board[StartPos] = null;

            Piece promotedPiece;

            if (PromotionType == PieceTypes.Queen)
            {
                promotedPiece = new Queen(pawn.Colour);
            }
            else if (PromotionType == PieceTypes.Rook)
            {
                promotedPiece = new Rook(pawn.Colour);
            }
            else if (PromotionType == PieceTypes.Bishop)
            {
                promotedPiece = new Bishop(pawn.Colour);
            }
            else if (PromotionType == PieceTypes.Knight)
            {
                promotedPiece = new Knight(pawn.Colour);
            }
            else
            {
                throw new ArgumentException("Invalid promotion piece");
            }

            promotedPiece.Moved = true;
            board[EndPos] = promotedPiece;
        }
    }
}

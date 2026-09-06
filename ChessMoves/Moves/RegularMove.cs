using ChessGame.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class RegularMove : Move
    {
        public override Position StartPos { get; }
        public override Position EndPos { get; }

        public RegularMove(Position start, Position end)
        {
            StartPos = start;
            EndPos = end;
        }

        public override void DoMove(Board board)
        {
            Piece piece = board[StartPos];
            board[EndPos] = piece;
            board[StartPos] = null;
            piece.Moved = true;
        }
    }
}

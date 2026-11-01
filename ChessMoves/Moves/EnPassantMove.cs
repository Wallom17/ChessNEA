using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class EnPassantMove : Move
    {
        public override Position StartPos { get; }
        public override Position EndPos { get; }
        public Position CapturedPawnPos { get; }

        public EnPassantMove(Position start, Position end, Position capturedPawn)
        {
            StartPos = start;
            EndPos = end;
            CapturedPawnPos = capturedPawn;
        }

        public override void DoMove(Board board) // does the move
        {
            Piece pawn = board[StartPos];

            board[CapturedPawnPos] = null;
            board[EndPos] = pawn;
            board[StartPos] = null;

            pawn.Moved = true;
        }

        public new bool Legal(Board board) // checks if the move is legal
        {
            Piece movingPawn = board[StartPos];
            Piece capturedPawn = board[CapturedPawnPos];

            board[StartPos] = null;
            board[CapturedPawnPos] = null;
            board[EndPos] = movingPawn;

            bool legal = !board.Checked(movingPawn.Colour);

            board[EndPos] = null;
            board[StartPos] = movingPawn;
            board[CapturedPawnPos] = capturedPawn;

            return legal;
        }
    }
}
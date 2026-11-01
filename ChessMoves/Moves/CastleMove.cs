using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame.Moves
{
    public class CastleMove : Move
    {
        public override Position StartPos { get; }
        public override Position EndPos { get; }

        public Position RookStart { get; }
        public Position RookEnd { get; }

        public CastleMove(Position kingStart, Position kingEnd, Position rookStart, Position rookEnd)
        {
            StartPos = kingStart;
            EndPos = kingEnd;
            RookStart = rookStart;
            RookEnd = rookEnd;
        }

        public override void DoMove(Board board) // castles
        {
            Piece king = board[StartPos];
            Piece rook = board[RookStart];

            board[StartPos] = null;
            board[RookStart] = null;

            board[EndPos] = king;
            board[RookEnd] = rook;

            king.Moved = true;
            rook.Moved = true;
        }

        public override bool Legal(Board board) // checks the legality of the move
        {
            Piece king = board[StartPos];
            Piece rook = board[RookStart];

            if (king == null || rook == null)
                return false;

            if (king.Type != PieceTypes.King || rook.Type != PieceTypes.Rook)
                return false;

            if (king.Colour != rook.Colour || king.Moved || rook.Moved)
                return false;

            if (board.Checked(king.Colour))
                return false;

            if (!PathClear(board, StartPos, EndPos, RookStart))
                return false;

            if (!PathClear(board, RookStart, RookEnd, StartPos))
                return false;

            board[StartPos] = null;
            board[RookStart] = null;

            try
            {
                int direction = Math.Sign(EndPos.column - StartPos.column);

                if (direction == 0)
                {
                    board[StartPos] = king;

                    if (board.Checked(king.Colour))
                        return false;

                    board[StartPos] = null;
                }
                else
                {
                    for (int column = StartPos.column + direction; ; column += direction)
                    {
                        if (direction > 0 && column > EndPos.column)
                            break;

                        if (direction < 0 && column < EndPos.column)
                            break;

                        Position testPosition = new Position(StartPos.row, column);

                        board[testPosition] = king;

                        bool inCheck = board.Checked(king.Colour);

                        board[testPosition] = null;

                        if (inCheck)
                            return false;
                    }
                }

                board[EndPos] = king;
                board[RookEnd] = rook;

                return !board.Checked(king.Colour);
            }
            finally
            {
                board[EndPos] = null;
                board[RookEnd] = null;

                board[StartPos] = king;
                board[RookStart] = rook;
            }
        }

        private bool PathClear(Board board, Position start, Position end, Position exception) // checks if it is possible to castle, so there is no pieces in the way
        { 
            int direction = Math.Sign(end.column - start.column);

            if (direction == 0)
                return true;

            for (int column = start.column + direction; ; column += direction)
            {
                if (direction > 0 && column > end.column)
                    break;

                if (direction < 0 && column < end.column)
                    break;

                Position position = new Position(start.row, column);

                if (position.row == exception.row && position.column == exception.column)
                    continue;

                if (board.CheckPiece(position))
                    return false;
            }

            return true;
        }
    }
}

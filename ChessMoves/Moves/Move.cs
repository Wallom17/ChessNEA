using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public abstract class Move
    {
        public abstract Position StartPos { get; } // where the piece starts
        public abstract Position EndPos { get; } // where the piece is moving to

        public abstract void DoMove(Board board);

        public bool Legal(Board board) // checks if move is legal
        {
            Piece temp = board[EndPos];

            board[EndPos] = board[StartPos];
            board[StartPos] = null;

            bool result = !board.Checked(board[EndPos].Colour);

            board[StartPos] = board[EndPos];
            board[EndPos] = temp;

            return result;
        }

    }
}

using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public abstract class Piece
    {
        public abstract PieceTypes Type { get; }
        public abstract PlayerColour Colour { get; }
        public bool Moved { get; set; } = false;

        public abstract IEnumerable<Move> GetMove(Position start, Board board);

        protected IEnumerable<Position> PossibleMovesInDir(Position start, Board board, Directions dir) // finds all possible moves in a given direction
        {
            for (Position pos = Position.NewPosition(start, dir); Board.IsIn(pos); pos = Position.NewPosition(pos, dir))
            {
                if (Board.CheckPiece(pos))
                {
                    Piece piece = board[pos];
                    if (piece.Colour != Colour)
                    {
                        yield return pos;
                    }
                    yield break;
                }
                yield return pos;
            }
        }
        protected IEnumerable<Position> PossibleMovesInDir(Position start, Board board, Directions[] dirs)
        {
            if (dirs == null)
                yield break;

            foreach (Directions dir in dirs)
            {
                foreach (Position pos in PossibleMovesInDir(start, board, dir))
                    yield return pos;
            }
        }

    }
}

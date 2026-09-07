using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class King : Piece
    {
        public override PieceTypes Type => PieceTypes.King;
        public override PlayerColour Colour { get; }
        private static readonly Directions[] dirs = new Directions[] { Directions.UpLeft, Directions.UpRight, Directions.DownLeft, Directions.DownRight, Directions.Up, Directions.Down, Directions.Left, Directions.Right }; // all directions a king can move

        public King(PlayerColour colour)
        {
            this.Colour = colour;
        }

        private IEnumerable<Position> Move(Position start, Board board) // finds all possible moves a king can make
        {
            foreach (Directions dir in dirs)
            {
                Position end = Position.NewPosition(start, dir);

                if (Board.IsIn(end))
                {
                    if (!board.CheckPiece(end) || board[end].Colour != Colour)
                    {
                        yield return end;
                    }
                }
            }
        }
        public override IEnumerable<Move> GetMove(Position start, Board board)
        {
            foreach(Position end in Move(start, board))
            {
                yield return new RegularMove(start, end);
            }
        }

        public override bool Check(Position start, Board board)
        {
            foreach (Move move in GetMove(start, board))
            {
                Piece piece = board[move.EndPos];

                if (piece != null && piece.Type == PieceTypes.King)
                {
                    return true;
                }
            }

            return false;
        }

    }
}

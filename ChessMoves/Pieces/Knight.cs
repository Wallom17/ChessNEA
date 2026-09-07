using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Knight : Piece
    {
        public override PieceTypes Type => PieceTypes.Knight;
        public override PlayerColour Colour { get; }

        public Knight(PlayerColour colour)
        {
            this.Colour = colour;
        }

        private static IEnumerable<Position> PossibleMoves(Position start) // finds every possible move a knight can make
        {
            Directions u = Directions.Up;
            Directions d = Directions.Down;
            Directions l = Directions.Left;
            Directions r = Directions.Right;

            yield return Position.NewPosition(start, (u * 2) + r);
            yield return Position.NewPosition(start, (u * 2) + l);
            yield return Position.NewPosition(start, (d * 2) + r);
            yield return Position.NewPosition(start, (d * 2) + l);
            yield return Position.NewPosition(start, (r * 2) + u);
            yield return Position.NewPosition(start, (r * 2) + d);
            yield return Position.NewPosition(start, (l * 2) + u);
            yield return Position.NewPosition(start, (l * 2) + d);
        }

        private IEnumerable<Position> Move(Position start, Board board) // Finds all legal moves a knight can make (within the board, and not capturing a ally piece)
        {
            foreach (Position pos in PossibleMoves(start))
            {
                if (Board.IsIn(pos) && (!board.CheckPiece(pos) || board[pos].Colour != Colour))
                {
                    yield return pos;
                }
            }
        }

        public override IEnumerable<Move> GetMove(Position start, Board board)
        {
            return Move(start, board)
                .Select(end => new RegularMove(start, end));
        }
    }
}

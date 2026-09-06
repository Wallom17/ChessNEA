using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Pawn : Piece
    {
        public override PieceTypes Type => PieceTypes.Pawn;
        public override PlayerColour Colour { get; }
        private readonly Directions forward;

        public Pawn(PlayerColour colour)
        {
            this.Colour = colour;

            if (colour == PlayerColour.White)
            {
                forward = Directions.Up;
            }
            else if (colour == PlayerColour.Black)
            {
                forward = Directions.Down;
            }
        }
        private bool CheckMove(Position pos, Board board)
        {   
            return Board.IsIn(pos) && !(board.CheckPiece(pos));
        }
        private bool CheckCapture(Position pos, Board board)
        {
            if (!(Board.IsIn(pos)) || !(board.CheckPiece(pos)))
            {
                return false;
            }
            return board[pos].Colour != Colour;
        }
        private IEnumerable<Move> PossibleForwardMoves(Position start, Board board)
        {
            Position oneSquare = Position.NewPosition(start, forward);
            if (CheckMove(oneSquare, board))
            {
                yield return new RegularMove(start, oneSquare);

                Position twoSquares = Position.NewPosition(oneSquare, forward);
                if (!Moved && CheckMove(twoSquares, board))
                {
                    yield return new RegularMove(start, twoSquares);
                }
            }
        }
        private IEnumerable<Move> PossibleCaptures(Position start, Board board)
        {
            foreach ( Directions dir in new Directions[] { Directions.Left, Directions.Right})
            {
                Position end = Position.NewPosition(start, forward);
                end = Position.NewPosition(end, dir);

                if (CheckCapture(end, board))
                {
                    yield return new RegularMove(start, end);
                }
            }
        }
        public override IEnumerable<Move> GetMove(Position start, Board board)
        {
            return PossibleForwardMoves(start, board).Concat(PossibleCaptures(start, board));
        }
    }
}

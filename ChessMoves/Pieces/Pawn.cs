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

            // figures out which direction is forward dedpending on the pawn's colour

            if (colour == PlayerColour.White)
            {
                forward = Directions.Up;
            }
            else if (colour == PlayerColour.Black)
            {
                forward = Directions.Down;
            }
        }
        private bool CheckMove(Position pos, Board board) // checks if the move is legal
        {   
            return Board.IsIn(pos) && !(board.CheckPiece(pos));
        }
        private bool CheckCapture(Position pos, Board board) // checks if the capture is legal
        {
            if (!(Board.IsIn(pos)) || !(board.CheckPiece(pos)))
            {
                return false;
            }
            return board[pos].Colour != Colour;
        }
        private IEnumerable<Move> PossibleForwardMoves(Position start, Board board) // finds all possible moves a pawn can make
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
        private IEnumerable<Move> PossibleCaptures(Position start, Board board) // checks for any captures a pawn can make (diagonal)
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

        public override bool Check(Position start, Board board) // overrides check method to only work on diagonals, as pawns can only capture/give checks on a diagonal
        {
            foreach (Move move in PossibleCaptures(start, board))
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

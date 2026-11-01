using ChessGame.enums;
using ChessGame.Moves;
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
        public IEnumerable<Move> GetEnPassantMoves(Position start, Board board, Position target, Position capturedPawn) // gets possible en passant moves
        {
            if (target == null || capturedPawn == null)
                yield break;

            foreach (Directions dir in new[]{Directions.Left, Directions.Right})
            {
                Position end = Position.NewPosition(Position.NewPosition(start, forward), dir);

                if (end == target)
                {
                    Piece captured = board[capturedPawn];

                    if (captured is Pawn && captured.Colour != Colour)
                    {
                        yield return new EnPassantMove(start, end, capturedPawn);
                    }
                }
            }
        }
        private IEnumerable<Move> PromotionMoves(Position start, Board board) 
        {
            foreach (Move move in PossibleForwardMoves(start, board).Concat(PossibleCaptures(start, board)))
            {
                int finalRow;

                if (Colour == PlayerColour.White)
                {
                    finalRow = 0;
                }
                else
                {
                    finalRow = 7;
                }

                if (move.EndPos.row == finalRow)
                {
                    foreach (PieceTypes type in new[]{PieceTypes.Queen,PieceTypes.Rook,PieceTypes.Bishop,PieceTypes.Knight})
                    {
                        yield return new PromotionMove(start, move.EndPos, type);
                    }
                }
                else
                {
                    yield return move;
                }
            }
        }
        public override IEnumerable<Move> GetMove(Position start, Board board)
        {
            return PromotionMoves(start, board);
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

using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Rook : Piece
    {
        public override PieceTypes Type => PieceTypes.Rook;
        public override PlayerColour Colour { get; }
        private static readonly Directions[] dirs = new Directions[] { Directions.Up, Directions.Down, Directions.Left, Directions.Right };

        public Rook(PlayerColour colour)
        {
            this.Colour = colour;
        }
        public override IEnumerable<Move> GetMove(Position start, Board board)
        {
            return PossibleMovesInDir(start, board, dirs)
                   .Select(end => new RegularMove(start, end));
        }

    }
}

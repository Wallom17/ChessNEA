using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ChessGame
{
    public class Bishop : Piece
    {
        public override PieceTypes Type => PieceTypes.Bishop;
        public override PlayerColour Colour { get; }

        private static readonly Directions[] dirs = new Directions[] { Directions.UpLeft, Directions.UpRight, Directions.DownLeft, Directions.DownRight}; // all directions a bishop can move

        public Bishop(PlayerColour colour)
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

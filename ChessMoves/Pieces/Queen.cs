using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Queen : Piece
    {
        public override PieceTypes Type => PieceTypes.Queen;
        public override PlayerColour Colour { get; }
        private static readonly Directions[] dirs = new Directions[] { Directions.UpLeft, Directions.UpRight, Directions.DownLeft, Directions.DownRight, Directions.Up, Directions.Down, Directions.Left, Directions.Right }; // all possible directions a queen can move
        public Queen(PlayerColour colour)
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

using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame.Pieces
{
    public class Queen : Piece
    {
        public override PieceTypes Type => PieceTypes.Queen;
        public override PlayerColour Colour { get; }

        public Queen(PlayerColour colour)
        {
            this.Colour = colour;
        }

    }
}

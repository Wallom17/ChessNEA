using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame.Pieces
{
    public class Rook : Piece
    {
        public override PieceTypes Type => PieceTypes.Rook;
        public override PlayerColour Colour { get; }

        public Rook(PlayerColour colour)
        {
            this.Colour = colour;
        }

    }
}

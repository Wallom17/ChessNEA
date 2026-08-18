using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame.Pieces
{
    public class Bishop : Piece
    {
        public override PieceTypes Type => PieceTypes.Bishop;
        public override PlayerColour Colour { get; }

        public Bishop(PlayerColour colour)
        {
            this.Colour = colour;
        }

    }
}

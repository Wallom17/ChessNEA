using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class King : Piece
    {
        public override PieceTypes Type => PieceTypes.King;
        public override PlayerColour Colour { get; }

        public King(PlayerColour colour)
        {
            this.Colour = colour;
        }

    }
}

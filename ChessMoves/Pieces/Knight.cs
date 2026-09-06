using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Knight : Piece
    {
        public override PieceTypes Type => PieceTypes.Knight;
        public override PlayerColour Colour { get; }

        public Knight(PlayerColour colour)
        {
            this.Colour = colour;
        }

    }
}

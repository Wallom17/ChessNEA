using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame.Pieces
{
    public class Pawn : Piece
    {
        public override PieceTypes Type => PieceTypes.Pawn;
        public override PlayerColour Colour { get; }

        public Pawn(PlayerColour colour)
        {
            this.Colour = colour;
        }
        
    }
}

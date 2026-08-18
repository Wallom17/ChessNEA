using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public abstract class Piece
    {
        public abstract PieceTypes Type { get; }
        public abstract PlayerColour Colour { get;  }
        public bool Moved { get; set; } = false;


    }
}

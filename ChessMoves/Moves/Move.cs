using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public abstract class Move
    {
        public abstract Position StartPos { get; }
        public abstract Position EndPos { get; }

        public abstract void DoMove(Board board);
    }
}

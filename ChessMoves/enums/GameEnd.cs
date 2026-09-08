using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame.enums
{
    public enum GameEnd // every way a chess game can end
    {
        Checkmate,
        Stalemate,
        InsufficientMaterial,
        FiftyMoves,
        ThreefoldRepetition
    }
}

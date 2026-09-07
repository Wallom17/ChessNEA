using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame.enums
{
    public enum PlayerColour
    {
        None,
        White,
        Black
    }
    public static class Player
    {
        public static PlayerColour Opponent(this PlayerColour player) // gets the colour of the opponent on the current turn
        {
            switch (player)
            {
                case PlayerColour.White:
                    return PlayerColour.Black;
                case PlayerColour.Black:
                    return PlayerColour.White;
                default:
                    return PlayerColour.None;
            }
        }
    }
}

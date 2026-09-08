using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Winner
    {
        public PlayerColour Won { get; }
        public GameEnd How {  get; }

        public Winner(PlayerColour winner, GameEnd how)
        {
            Won = winner;
            How = how;
        }

        public static Winner Win(PlayerColour winner)
        {
            return new Winner(winner, GameEnd.Checkmate);
        }

        public static Winner Draw(GameEnd how)
        {
            return new Winner(PlayerColour.None, how);
        }
    }
}

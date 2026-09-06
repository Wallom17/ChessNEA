using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class GameState
    {
        public Board Board { get; }
        public PlayerColour CurrentTurn { get; private set; }

        public GameState(PlayerColour player, Board board)
        {
            CurrentTurn = player;
            Board = board;
        }

    }
}

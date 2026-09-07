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

        public IEnumerable<Move> LegalMoves(Position pos)
        {
            if (!Board.CheckPiece(pos) || Board[pos].Colour != CurrentTurn)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[pos];
            return piece.GetMove(pos, Board);
        }

        public void MakeMove(Move move)
        {
            move.DoMove(Board);
            CurrentTurn = CurrentTurn.Opponent();
        }
    }
}

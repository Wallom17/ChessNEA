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

        public IEnumerable<Move> LegalMoves(Position pos) // checks for all legal moves
        {
            if (!Board.CheckPiece(pos) || Board[pos].Colour != CurrentTurn)
            {
                yield break;
            }

            Piece piece = Board[pos];
            IEnumerable<Move> allMoves = piece.GetMove(pos, Board); // gives all moves

            foreach (Move move in allMoves) // gives all legal moves
            {
                if (move.Legal(Board))
                {
                    yield return move;
                }
            }
        }


        public void MakeMove(Move move) // does the move
        {
            move.DoMove(Board);
            CurrentTurn = CurrentTurn.Opponent();
        }
    }
}

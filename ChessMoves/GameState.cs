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
        public Winner Winner { get; private set; } = null;

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
            GameOver();
        }

        public IEnumerable<Move> AllCurrentPlayerMoves(PlayerColour player)
        {
            List<Move> possibleMoves = new List<Move>();

            foreach (Position pos in Board.PlayerPieces(player))
            {
                Piece piece = Board[pos];

                foreach (Move move in piece.GetMove(pos, Board))
                {
                    if (move.Legal(Board))
                    {
                        possibleMoves.Add(move);
                    }
                }
            }

            return possibleMoves;
        }

        private void GameOver()
        {
            if (!AllCurrentPlayerMoves(CurrentTurn).Any())
            {
                if (Board.Checked(CurrentTurn))
                {
                    Winner = Winner.Win(CurrentTurn.Opponent());
                }
                else
                {
                    Winner = Winner.Draw(GameEnd.Stalemate);

                }
            }
        }

    }
}

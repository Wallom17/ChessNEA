using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class ChessBot
    {
        private const int MateScore = 100000;

        private readonly Dictionary<PieceTypes, int> pieceValues = new Dictionary<PieceTypes, int>() // values for each piece type
            {
                { PieceTypes.Pawn, 100 },
                { PieceTypes.Knight, 320 },
                { PieceTypes.Bishop, 330 },
                { PieceTypes.Rook, 500 },
                { PieceTypes.Queen, 900 },
                { PieceTypes.King, 20000 }
            };

        public Move FindBestMove(GameState state, int depth = 3)
        {
            List<Move> moves = GetLegalMoves(state);

            if (moves.Count == 0)
            {
                return null;
            }

            Move bestMove = null;
            int bestScore = int.MinValue;

            int alpha = int.MinValue + 1;
            int beta = int.MaxValue;

            foreach (Move move in moves)
            {
                GameState newState = new GameState(state);
                newState.MakeMove(move);

                int score = -Negamax(newState, depth - 1, -beta, -alpha, 1);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }

                alpha = Math.Max(alpha, score);
            }

            return bestMove;
        }

        private int Negamax(GameState state, int depth, int alpha, int beta, int ply)
        {
            List<Move> moves = GetLegalMoves(state);

            // Checks for checkmate or stalemate.
            if (moves.Count == 0)
            {
                if (state.Board.Checked(state.CurrentTurn))
                {
                    return -MateScore + ply;
                }

                return 0;
            }

            if (depth == 0)
            {
                return Evaluate(state);
            }

            int bestScore = int.MinValue;

            foreach (Move move in moves)
            {
                GameState newState = new GameState(state);
                newState.MakeMove(move);

                int score = -Negamax(newState, depth - 1, -beta, -alpha, ply + 1
                );

                bestScore = Math.Max(bestScore, score);
                alpha = Math.Max(alpha, score);

                // Alpha-beta pruning to optimise the algorithm
                if (alpha >= beta)
                {
                    break;
                }
            }

            return bestScore;
        }

        private int Evaluate(GameState state) // evaluates the given position
        {
            int score = 0;

            foreach (Position pos in state.Board.AllPiecePos())
            {
                Piece piece = state.Board[pos];
                int value = pieceValues[piece.Type];

                if (piece.Colour == PlayerColour.White)
                {
                    score += value;
                }
                else
                {
                    score -= value;
                }
            }

            // Returns the score from the current player
            if (state.CurrentTurn == PlayerColour.Black)
            {
                score = -score;
            }

            return score;
        }

        private List<Move> GetLegalMoves(GameState state)
        {
            List<Move> moves = new List<Move>();

            foreach (Position pos in state.Board.PlayerPieces(state.CurrentTurn))
            {
                moves.AddRange(state.LegalMoves(pos));
            }

            return moves;
        }
    }
}


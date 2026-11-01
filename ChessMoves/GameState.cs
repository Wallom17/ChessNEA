using ChessGame.enums;
using ChessGame.Moves;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ChessGame
{
    public class GameState
    {
        public Board Board { get; }
        public PlayerColour CurrentTurn { get; private set; }
        public Winner Winner { get; private set; } = null;
        public int HalfMoveClock { get; private set; } = 0;
        public Position EnPassantCapture { get; private set; }
        public Position EnPassantPawn { get; private set; }
        // Starting positions of the kings
        private Position whiteKingStart;
        private Position blackKingStart;

        // Starting positions of the rooks
        private Position whiteKingSideRook;
        private Position whiteQueenSideRook;
        private Position blackKingSideRook;
        private Position blackQueenSideRook;

        // Shows if it is possible to castle either king or queen side
        private bool whiteKingSideAvailable;
        private bool whiteQueenSideAvailable;
        private bool blackKingSideAvailable;
        private bool blackQueenSideAvailable;

        public GameState(PlayerColour player, Board board)
        {
            CurrentTurn = player;
            Board = board;

            InitialiseCastling();
        }
        public GameState(GameState state)
        {
            Board = state.Board.Copy();
            CurrentTurn = state.CurrentTurn;
            Winner = state.Winner;

            if (state.EnPassantCapture == null)
            {
                EnPassantCapture = null;
            }
            else
            {
                EnPassantCapture = new Position(state.EnPassantCapture.row, state.EnPassantCapture.column);
            }

            if (state.EnPassantPawn == null)
            {
                EnPassantPawn = null;
            }
            else
            {
                EnPassantPawn = new Position(state.EnPassantPawn.row, state.EnPassantPawn.column);
            }

            whiteKingStart = CopyPosition(state.whiteKingStart);
            blackKingStart = CopyPosition(state.blackKingStart);

            whiteKingSideRook = CopyPosition(state.whiteKingSideRook);
            whiteQueenSideRook = CopyPosition(state.whiteQueenSideRook);
            blackKingSideRook = CopyPosition(state.blackKingSideRook);
            blackQueenSideRook = CopyPosition(state.blackQueenSideRook);

            whiteKingSideAvailable = state.whiteKingSideAvailable;
            whiteQueenSideAvailable = state.whiteQueenSideAvailable;
            blackKingSideAvailable = state.blackKingSideAvailable;
            blackQueenSideAvailable = state.blackQueenSideAvailable;
        }
        private Position CopyPosition(Position pos)
        {
            if (pos == null)
            {
                return null;
            }

            return new Position(pos.row, pos.column);
        }

        public IEnumerable<Move> LegalMoves(Position pos) // checks for all legal moves
        {
            if (!Board.CheckPiece(pos) ||
                Board[pos].Colour != CurrentTurn)
            {
                yield break;
            }

            Piece piece = Board[pos];

            // Normal moves
            foreach (Move move in piece.GetMove(pos, Board))
            {
                if (move.Legal(Board))
                {
                    yield return move;
                }
            }

            // En passant moves
            if (piece.Type == PieceTypes.Pawn)
            {
                foreach (Move move in EnPassantMoves(pos))
                {
                    if (move.Legal(Board))
                    {
                        yield return move;
                    }
                }
            }

            // Castling moves
            if (piece.Type == PieceTypes.King)
            {
                foreach (Move move in CastleMoves(pos))
                {
                    yield return move;
                }
            }
        }

        private IEnumerable<Move> EnPassantMoves(Position pos) // checks for en passant moves
        {
            if (EnPassantCapture == null || EnPassantPawn == null)
            {
                yield break;
            }

            Piece piece = Board[pos];

            if (piece == null || piece.Type != PieceTypes.Pawn)
            {
                yield break;
            }

            int direction;

            if (piece.Colour == PlayerColour.White)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }

            if (EnPassantCapture.row != pos.row + direction || Math.Abs(EnPassantCapture.column - pos.column) != 1)
            {
                yield break;
            }

            if (Board.CheckPiece(EnPassantCapture))
            {
                yield break;
            }

            if (EnPassantPawn.row != pos.row || EnPassantPawn.column != EnPassantCapture.column)
            {
                yield break;
            }

            Piece capturedPawn = Board[EnPassantPawn];

            if (capturedPawn == null || capturedPawn.Type != PieceTypes.Pawn || capturedPawn.Colour == piece.Colour)
            {
                yield break;
            }

            yield return new EnPassantMove(pos, EnPassantCapture, EnPassantPawn);
        }

        public void MakeMove(Move move) // does the move
        {
            Piece Piece = Board[move.StartPos];
            bool isPawnMove = Piece is Pawn;
            bool isCapture = Board[move.EndPos] != null;

            if (move is EnPassantMove)
            {
                isCapture = true;
            }

            if (isPawnMove || isCapture)
            {
                HalfMoveClock = 0;
            }
            else
            {
                HalfMoveClock++;
            }

            EnPassantCapture = null;
            EnPassantPawn = null;

            Piece piece = Board[move.StartPos];

            if (piece is Pawn && Math.Abs(move.StartPos.row - move.EndPos.row) == 2) // en passant
            {
                EnPassantPawn = move.EndPos;
                EnPassantCapture = new Position((move.StartPos.row + move.EndPos.row) / 2, move.StartPos.column);
            }

            UpdateCastlingRights(move);
            move.DoMove(Board);
            CurrentTurn = CurrentTurn.Opponent();
            GameOver();
        }

        public IEnumerable<Move> AllCurrentPlayerMoves(PlayerColour player) // finds every possible move of a given player
        {
            List<Move> possibleMoves = new List<Move>();

            foreach (Position pos in Board.PlayerPieces(player))
            {
                Piece piece = Board[pos];

                foreach (Move move in piece.GetMove(pos, Board)) // normal moves
                {
                    if (move.Legal(Board))
                    {
                        possibleMoves.Add(move);
                    }
                }

                if (piece.Type == PieceTypes.Pawn) // en passant moves
                {
                    foreach (Move move in EnPassantMoves(pos))
                    {
                        if (move.Legal(Board))
                        {
                            possibleMoves.Add(move);
                        }
                    }
                }

                if (piece.Type == PieceTypes.King) // castling moves
                {
                    foreach (Move move in CastleMoves(pos))
                    {
                        possibleMoves.Add(move);
                    }
                }
            }

            return possibleMoves;
        }

        private void GameOver() // checks for checkmate or stalemate (other ways of ending game will be added, e.g threefold repetition, 50 moves rule, insufficient material)
        {
            // checkmate or stalemate
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
            // fifty move rule
            if (HalfMoveClock >= 100)
            {
                Winner = Winner.Draw(GameEnd.FiftyMoves);
                return;
            }
            // insufficient material
            if (InsufficientMaterial())
            {
                Winner = Winner.Draw(GameEnd.InsufficientMaterial);
                return;
            }
        }
        private void InitialiseCastling()
        {
            for (int row = 0; row < 8; row += 7)
            {
                PlayerColour colour;

                if (row == 7)
                {
                    colour = PlayerColour.White;
                }
                else
                {
                    colour = PlayerColour.Black;
                }

                Position kingPosition = null;
                List<Position> rooks = new List<Position>();

                for (int column = 0; column < 8; column++)
                {
                    Position position = new Position(row, column);
                    Piece piece = Board[position];

                    if (piece == null || piece.Colour != colour)
                    {
                        continue;
                    }

                    if (piece.Type == PieceTypes.King)
                    {
                        kingPosition = position;
                    }

                    if (piece.Type == PieceTypes.Rook)
                    {
                        rooks.Add(position);
                    }
                }

                if (kingPosition == null)
                {
                    continue;
                }

                // Finds the nearest rook on either side of the king.
                Position leftRook = rooks.Where(r => r.column < kingPosition.column).OrderByDescending(r => r.column).FirstOrDefault();
                Position rightRook = rooks.Where(r => r.column > kingPosition.column).OrderBy(r => r.column).FirstOrDefault();

                if (colour == PlayerColour.White)
                {
                    whiteKingStart = kingPosition;
                    whiteQueenSideRook = leftRook;
                    whiteKingSideRook = rightRook;
                    whiteQueenSideAvailable = leftRook != null;
                    whiteKingSideAvailable = rightRook != null;
                }
                else
                {
                    blackKingStart = kingPosition;
                    blackQueenSideRook = leftRook;
                    blackKingSideRook = rightRook;
                    blackQueenSideAvailable = leftRook != null;
                    blackKingSideAvailable = rightRook != null;
                }
            }
        }
        private IEnumerable<Move> CastleMoves(Position start)
        {
            Piece king = Board[start];

            if (king == null || king.Type != PieceTypes.King)
                yield break;

            if (king.Moved || Board.Checked(king.Colour))
                yield break;

            Position kingSideRook;
            Position queenSideRook;

            bool kingSideAvailable;
            bool queenSideAvailable;

            if (king.Colour == PlayerColour.White)
            {
                kingSideRook = whiteKingSideRook;
                queenSideRook = whiteQueenSideRook;

                kingSideAvailable = whiteKingSideAvailable;
                queenSideAvailable = whiteQueenSideAvailable;
            }
            else
            {
                kingSideRook = blackKingSideRook;
                queenSideRook = blackQueenSideRook;

                kingSideAvailable = blackKingSideAvailable;
                queenSideAvailable = blackQueenSideAvailable;
            }

            if (kingSideAvailable && kingSideRook != null)
            {
                Position kingEnd = new Position(start.row, 6);
                Position rookEnd = new Position(start.row, 5);

                CastleMove move = new CastleMove(
                    start, kingEnd, kingSideRook, rookEnd
                );

                if (move.Legal(Board))
                    yield return move;
            }

            if (queenSideAvailable && queenSideRook != null)
            {
                Position kingEnd = new Position(start.row, 2);
                Position rookEnd = new Position(start.row, 3);

                CastleMove move = new CastleMove(
                    start, kingEnd, queenSideRook, rookEnd
                );

                if (move.Legal(Board))
                    yield return move;
            }
        }
        private void UpdateCastlingRights(Move move)
        {
            Piece movingPiece = Board[move.StartPos];
            Piece capturedPiece = Board[move.EndPos];

            // moving a king removes its castling rights on both sides
            if (movingPiece.Type == PieceTypes.King)
            {
                if (movingPiece.Colour == PlayerColour.White)
                {
                    whiteKingSideAvailable = false;
                    whiteQueenSideAvailable = false;
                }
                else
                {
                    blackKingSideAvailable = false;
                    blackQueenSideAvailable = false;
                }
            }

            // a rook moving removes its castling rights on the one side
            if (movingPiece.Type == PieceTypes.Rook)
            {
                RemoveRookCastlingRight(move.StartPos, movingPiece.Colour);
            }

            // a rook getting captured removes its castling rights on the one side
            if (capturedPiece != null && capturedPiece.Type == PieceTypes.Rook)
            {
                RemoveRookCastlingRight(move.EndPos, capturedPiece.Colour);
            }
        }
        private void RemoveRookCastlingRight(Position position, PlayerColour colour)
        {
            if (colour == PlayerColour.White)
            {
                if (position == whiteKingSideRook)
                {
                    whiteKingSideAvailable = false;
                }

                if (position == whiteQueenSideRook)
                {
                    whiteQueenSideAvailable = false;
                }
            }
            else if (colour == PlayerColour.Black)
            {
                if (position == blackKingSideRook)
                {
                    blackKingSideAvailable = false;
                }

                if (position == blackQueenSideRook)
                {
                    blackQueenSideAvailable = false;
                }
            }
        }
        private bool InsufficientMaterial() // checks for insufficient material
        {
            List<Piece> pieces = new List<Piece>();

            foreach (Position pos in Board.AllPiecePos())
            {
                pieces.Add(Board[pos]);
            }

            // Removes the two kings.
            for (int i = pieces.Count - 1; i >= 0; i--)
            {
                if (pieces[i].Type == PieceTypes.King)
                {
                    pieces.RemoveAt(i);
                }
            }

            // Checks for king against king.
            if (pieces.Count == 0)
                return true;

            // Checks for king and bishop/knight against king.
            if (pieces.Count == 1)
            {
                if (pieces[0].Type == PieceTypes.Bishop || pieces[0].Type == PieceTypes.Knight)
                    return true;
            }

            // Checks for king and bishop against king and bishop.
            if (pieces.Count == 2)
            {
                if (pieces[0].Type == PieceTypes.Bishop && pieces[1].Type == PieceTypes.Bishop)
                {
                    List<Position> bishops = new List<Position>();

                    foreach (Position pos in Board.AllPiecePos())
                    {
                        if (Board[pos].Type == PieceTypes.Bishop)
                        {
                            bishops.Add(pos);
                        }
                    }

                    return bishops[0].GetSquareColour() == bishops[1].GetSquareColour();
                }
            }

            return false;
        }
    }
}

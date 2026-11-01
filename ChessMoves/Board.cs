using ChessGame.enums;
using ChessGame;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChessGame
{
    public class Board
    {
        private readonly Piece[,] PieceStorage = new Piece[8, 8]; // stores the entire 8x8 board

        // indexers to get and set the pieces
        public Piece this[int row, int col]
        {
            get { return PieceStorage[row, col]; }
            set { PieceStorage[row, col] = value; }
        }
        public Piece this[Position pos]
        {
            get { return  this[pos.row, pos.column]; }
            set {  this[pos.row, pos.column] = value; }
        }

        public static Board InitialBoard()
        {
            Board board = new Board();
            board.StartingPieces();
            return board;
        }

        private void StartingPieces() // adds starting pieces to board
        {
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                this[1, i] = new Pawn(PlayerColour.Black);
                this[6, i] = new Pawn(PlayerColour.White);
            }

            string[] pieces = new string[8];

            // Bishops need to be on opposite coloured squares
            int bishop1 = random.Next(0, 4) * 2;
            int bishop2 = random.Next(0, 4) * 2 + 1;

            pieces[bishop1] = "Bishop";
            pieces[bishop2] = "Bishop";

            // adds the queen into a random square
            List<int> empty = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                if (pieces[i] == null)
                {
                    empty.Add(i);
                }
            }

            int queenPosition = empty[random.Next(empty.Count)];
            pieces[queenPosition] = "Queen";

            // adds first knight to a random square
            empty = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                if (pieces[i] == null)
                {
                    empty.Add(i);
                }
            }

            int knight1 = empty[random.Next(empty.Count)];
            pieces[knight1] = "Knight";

            // adds second knight to a random square
            empty = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                if (pieces[i] == null)
                {
                    empty.Add(i);
                }
            }

            int knight2 = empty[random.Next(empty.Count)];
            pieces[knight2] = "Knight";

            // adds remaining pieces, rook, king and rook
            empty = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                if (pieces[i] == null)
                {
                    empty.Add(i);
                }
            }

            pieces[empty[0]] = "Rook";
            pieces[empty[1]] = "King";
            pieces[empty[2]] = "Rook";

            for (int i = 0; i < 8; i++)
            {
                switch (pieces[i])
                {
                    case "King":
                        this[0, i] = new King(PlayerColour.Black);
                        this[7, i] = new King(PlayerColour.White);
                        break;

                    case "Queen":
                        this[0, i] = new Queen(PlayerColour.Black);
                        this[7, i] = new Queen(PlayerColour.White);
                        break;

                    case "Rook":
                        this[0, i] = new Rook(PlayerColour.Black);
                        this[7, i] = new Rook(PlayerColour.White);
                        break;

                    case "Knight":
                        this[0, i] = new Knight(PlayerColour.Black);
                        this[7, i] = new Knight(PlayerColour.White);
                        break;

                    case "Bishop":
                        this[0, i] = new Bishop(PlayerColour.Black);
                        this[7, i] = new Bishop(PlayerColour.White);
                        break;
                }
            }
        }

        public static bool IsIn(Position pos) // checks if the position is on the board
        {
            if (pos.row >= 0 && pos.column >= 0 && pos.row < 8 && pos.column < 8)
            {
                return true;
            }
            return false;
        }
        public bool CheckPiece(Position pos) // checks if there is a piece on that square
        {
            return this[pos] != null;
        }

        public IEnumerable<Position> AllPiecePos() // gets all the positions of pieces
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Position pos = new Position(i, j);

                    if (CheckPiece(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }
        public IEnumerable<Position> PlayerPieces(PlayerColour player) // gets all the piece locations from a given player
        {
            foreach (Position pos in AllPiecePos())
            {
                if (this[pos].Colour == player)
                {
                    yield return pos;
                }
            }
        }

        public bool Checked(PlayerColour player) // checks if a player is in check
        {
            foreach (Position pos in PlayerPieces(player.Opponent()))
            {
                Piece piece = this[pos];

                if (piece.Check(pos, this))
                {
                    return true;
                }
            }

            return false;
        }

        public Position FindKing(PlayerColour colour) // finds the position the king is on the board
        {
            foreach (Position pos in PlayerPieces(colour))
            {
                if (this[pos].Type == PieceTypes.King)
                    return pos;
            }

            return null;
        }

        public IEnumerable<Position> FindRooks(PlayerColour colour) // finds the position of the rooks on the board
        {
            foreach (Position pos in PlayerPieces(colour))
            {
                if (this[pos].Type == PieceTypes.Rook)
                    yield return pos;
            }
        }

        public Board Copy() // copies the board
        {
            Board copy = new Board();

            foreach (Position pos in AllPiecePos())
            {
                Piece piece = this[pos];
                Piece newPiece = null;

                switch (piece.Type)
                {
                    case PieceTypes.King:
                        newPiece = new King(piece.Colour);
                        break;

                    case PieceTypes.Queen:
                        newPiece = new Queen(piece.Colour);
                        break;

                    case PieceTypes.Rook:
                        newPiece = new Rook(piece.Colour);
                        break;

                    case PieceTypes.Bishop:
                        newPiece = new Bishop(piece.Colour);
                        break;

                    case PieceTypes.Knight:
                        newPiece = new Knight(piece.Colour);
                        break;

                    case PieceTypes.Pawn:
                        newPiece = new Pawn(piece.Colour);
                        break;
                }

                newPiece.Moved = piece.Moved;
                copy[pos] = newPiece;
            }

            return copy;
        }

    }
}

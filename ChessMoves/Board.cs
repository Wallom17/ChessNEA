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

        private void StartingPieces() // adds the starting pieces to the board
        {
            Random r = new Random();

            for (int i = 0; i < 8; i++)
            {
                this[1, i] = new Pawn(PlayerColour.Black);
                this[6, i] = new Pawn(PlayerColour.White);
            }

            string[] AllPieces = {"King", "Queen", "Rook", "Rook", "Knight", "Knight", "Bishop", "Bishop" }; // shuffles the array to create a random back rank
            r.Shuffle(AllPieces.AsSpan());

            for (int i = 0;i < 8;i++)
            {
                if (AllPieces[i] == "King")
                {
                    this[0, i] = new King(PlayerColour.Black);
                    this[7, i] = new King(PlayerColour.White);
                }
                else if (AllPieces[i] == "Queen")
                {
                    this[0, i] = new Queen(PlayerColour.Black);
                    this[7, i] = new Queen(PlayerColour.White);
                }
                else if (AllPieces[i] == "Rook")
                {
                    this[0, i] = new Rook(PlayerColour.Black);
                    this[7, i] = new Rook(PlayerColour.White);
                }
                else if (AllPieces[i] == "Knight")
                {
                    this[0, i] = new Knight(PlayerColour.Black);
                    this[7, i] = new Knight(PlayerColour.White);
                }
                else if (AllPieces[i] == "Bishop")
                {
                    this[0, i] = new Bishop(PlayerColour.Black);
                    this[7, i] = new Bishop(PlayerColour.White);
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

    }
}

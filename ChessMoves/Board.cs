using ChessGame.Pieces;
using System;
using System.Collections.Generic;
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

        private void StartingPieces()
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
                }
                else if (AllPieces[i] == "Queen")
                {
                    this[0, i] = new Queen(PlayerColour.Black);
                }
                else if (AllPieces[i] == "Rook")
                {
                    this[0, i] = new Rook(PlayerColour.Black);
                }
                else if (AllPieces[i] == "Knight")
                {
                    this[0, i] = new Knight(PlayerColour.Black);
                }
                else if (AllPieces[i] == "Bishop")
                {
                    this[0, i] = new Bishop(PlayerColour.Black);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (AllPieces[i] == "King")
                {
                    this[7, i] = new King(PlayerColour.White);
                }
                else if (AllPieces[i] == "Queen")
                {
                    this[7, i] = new Queen(PlayerColour.White);
                }
                else if (AllPieces[i] == "Rook")
                {
                    this[7, i] = new Rook(PlayerColour.White);
                }
                else if (AllPieces[i] == "Knight")
                {
                    this[7, i] = new Knight(PlayerColour.White);
                }
                else if (AllPieces[i] == "Bishop")
                {
                    this[7, i] = new Bishop(PlayerColour.White);
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessGame.enums;
using ChessGame;

namespace ChessUI
{
    internal static class LoadImages
    {
        // dictionary for all piece assets
        private static readonly Dictionary<PieceTypes, ImageSource> white = new Dictionary<PieceTypes, ImageSource>
        {
            {PieceTypes.Pawn, LoadImage("Assets/PawnW.png") },
            {PieceTypes.Rook, LoadImage("Assets/RookW.png") },
            {PieceTypes.Bishop, LoadImage("Assets/BishopW.png") },
            {PieceTypes.Knight, LoadImage("Assets/KnightW.png") },
            {PieceTypes.Queen, LoadImage("Assets/QueenW.png") },
            {PieceTypes.King, LoadImage("Assets/KingW.png") }
        };
        private static readonly Dictionary<PieceTypes, ImageSource> black = new Dictionary<PieceTypes, ImageSource>
        {
            {PieceTypes.Pawn, LoadImage("Assets/PawnB.png") },
            {PieceTypes.Rook, LoadImage("Assets/RookB.png") },
            {PieceTypes.Bishop, LoadImage("Assets/BishopB.png") },
            {PieceTypes.Knight, LoadImage("Assets/KnightB.png") },
            {PieceTypes.Queen, LoadImage("Assets/QueenB.png") },
            {PieceTypes.King, LoadImage("Assets/KingB.png") }
        };

        private static ImageSource LoadImage(string file)
        {
            return new BitmapImage(new Uri(file, UriKind.Relative));
        }
        // gets the image based on the colour and piece type
        public static ImageSource GetImage(PlayerColour colour, PieceTypes types)
        {
            if (colour == PlayerColour.White)
            {
                return white[types];
            }
            else if (colour == PlayerColour.Black)
            { 
                return black[types];
            }
            else { return null; }
        }
        // overload for allowing piece to be given
        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
            {  return null; }
            else
            {
                return GetImage(piece.Colour, piece.Type);
            }
           
        }
    }
}

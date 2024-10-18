﻿using ChessLogic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess_UI
{
    public static class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new Dictionary<PieceType, ImageSource>()
        {
            { PieceType.Pawn, LoadImage("Assets/PawnW") },
            { PieceType.Bishop, LoadImage("Assets/BishopW.png") },
            { PieceType.Knight, LoadImage("Assets/KnightW.png") },
            { PieceType.Rook, LoadImage("Assets/RookW.png") },
            { PieceType.Queen, LoadImage("Assets/QueenW.png") },
            { PieceType.King, LoadImage("Assets/KingW.png") }
        };

        private static readonly Dictionary<PieceType, ImageSource> blackSources = new Dictionary<PieceType, ImageSource>()
        {
            { PieceType.Pawn, LoadImage("Assets/PawnB.png") },
            { PieceType.Bishop, LoadImage("Assets/BishopB.png") },
            { PieceType.Knight, LoadImage("Assets/KnightB.png") },
            { PieceType.Rook, LoadImage("Assets/RookB.png") },
            { PieceType.Queen, LoadImage("Assets/QueenB.png") },
            { PieceType.King, LoadImage("Assets/KingB.png") }
        };

        private static ImageSource LoadImage(string filepath)
        {
            return new BitmapImage(new Uri(filepath, UriKind.Relative));
        }

        public static ImageSource GetImage(Player color, PieceType pieceType)
        {
            return color switch
            {
                Player.White => whiteSources[pieceType],
                Player.Black => blackSources[pieceType],
                _ => null
            };
        }

        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }
            else
            {
                return GetImage(piece.Color, piece.Type);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Directions
    {
        // Predefine every possible direction
        public readonly static Directions Up = new Directions(-1, 0);
        public readonly static Directions Down = new Directions(1, 0);
        public readonly static Directions Left = new Directions(0, -1);
        public readonly static Directions Right = new Directions(0, 1);
        public readonly static Directions UpLeft = Up + Left;
        public readonly static Directions DownLeft = Down + Left;
        public readonly static Directions UpRight = Up + Right;
        public readonly static Directions DownRight = Down + Right;

        public int RowChange { get; }
        public int ColumnChange { get; }

        public Directions(int rowChange, int columnChange)
        {
            RowChange = rowChange;
            ColumnChange = columnChange;
        }

        // ovverides + and * operators to allow for easy addition and scaling of directions
        public static Directions operator +(Directions a, Directions b)
        {
            return new Directions(a.RowChange + b.RowChange, a.ColumnChange + b.ColumnChange);
        }
        public static Directions operator *(Directions a, Directions b)
        {
            return new Directions(a.RowChange * b.RowChange, a.ColumnChange * b.ColumnChange);
        }
    }
}

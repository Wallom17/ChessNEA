using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Position
    {
        public int row { get; }
        public int column { get; }

        // (0,0) is top left, (7,7) is bottom right

        public Position(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public string GetSquareColour() // gets the colour of the square based on its position
        {
            if ((row + column) % 2 == 0)
            {
                return "White";
            }
            else
            {
                return "Black";
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   row == position.row &&
                   column == position.column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(row, column);
        }

        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }
    
        public static Position NewPosition(Position start, Directions dir) // returns the new position after a given move
        {
            return new Position(start.row + dir.RowChange, start.column + dir.ColumnChange);
        }
    }
}

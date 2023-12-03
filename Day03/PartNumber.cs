using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03;

internal class PartNumber
{
    public PartNumber(int value, int row, int colStart, int colEnd)
    {
        this.Value = value; 
        this.Row = row;
        this.ColumnStart = colStart;
        this.ColumnEnd = colEnd;
    }
    internal int Value { get; }
    internal int Row { get; }
    internal int ColumnStart { get; }
    internal int ColumnEnd {  get; }

    internal bool IsAdjacent(Point point) => this.CheckNorthWest(point)
        || this.CheckNorth(point)
        || this.CheckNorthEast(point)
        || this.CheckWest(point)
        || this.CheckEast(point)
        || this.CheckSouthWest(point)
        || this.CheckSouth(point)
        || this.CheckSouthEast(point);

    internal bool CheckNorthWest(Point point) => this.Row == point.Y - 1 && this.ColumnEnd == point.X - 1;
    internal bool CheckNorth(Point point) => this.Row == point.Y - 1 && (this.ColumnStart <= point.X && this.ColumnEnd >= point.X);
    internal bool CheckNorthEast(Point point) => this.Row == point.Y - 1 && this.ColumnStart == point.X + 1;
    internal bool CheckWest(Point point) => this.Row == point.Y && this.ColumnEnd == point.X - 1;
    internal bool CheckEast(Point point) => this.Row == point.Y && this.ColumnStart == point.X + 1;
    internal bool CheckSouthWest(Point point) => this.Row == point.Y + 1 && this.ColumnEnd == point.X - 1;
    internal bool CheckSouth(Point point) => this.Row == point.Y + 1 && (this.ColumnStart <= point.X && this.ColumnEnd >= point.X);
    internal bool CheckSouthEast(Point point) => this.Row == point.Y + 1 && this.ColumnStart == point.X + 1;
}

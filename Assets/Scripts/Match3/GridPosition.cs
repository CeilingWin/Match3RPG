using System;

namespace Match3
{
    public class GridPosition
    {
        public int RowIndex;
        public int ColumnIndex;
        private readonly int hashInit;

        public GridPosition(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            hashInit = RowIndex * 10000 + ColumnIndex;
        }

        public static GridPosition Up { get; } = new GridPosition(1, 0);
        public static GridPosition Down { get; } = new GridPosition(-1, 0);
        public static GridPosition Left { get; } = new GridPosition(0, -1);
        public static GridPosition Right { get; } = new GridPosition(0, 1);
        public static GridPosition Zero { get; } = new GridPosition(0, 0);

        public static GridPosition operator +(GridPosition gr1, GridPosition gr2)
        {
            return new GridPosition(gr1.RowIndex + gr2.RowIndex, gr1.ColumnIndex + gr2.ColumnIndex);
        }
        
        public static GridPosition operator -(GridPosition gr1, GridPosition gr2)
        {
            return new GridPosition(gr1.RowIndex - gr2.RowIndex, gr1.ColumnIndex - gr2.ColumnIndex);
        }
        
        public static bool operator ==(GridPosition gr1, GridPosition gr2)
        {
            return gr1.ColumnIndex == gr2.ColumnIndex && gr1.RowIndex == gr2.RowIndex;
        }

        public static bool operator !=(GridPosition gr1, GridPosition gr2)
        {
            return gr1.ColumnIndex != gr2.ColumnIndex || gr1.RowIndex != gr2.RowIndex;
        }

        public static float Distance(GridPosition gr1, GridPosition gr2)
        {
            var d = gr1 - gr2;
            return (float) Math.Sqrt(d.ColumnIndex * d.ColumnIndex + d.RowIndex * d.RowIndex);
        }

        public override String ToString()
        {
            return "[row,column]".Replace("row", RowIndex.ToString()).Replace("column", ColumnIndex.ToString());
        }

        public override int GetHashCode()
        {
            return hashInit;
        }
    }
}
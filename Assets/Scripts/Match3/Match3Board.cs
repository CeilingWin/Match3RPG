using System;
using Unit;
using UnityEngine;

namespace Match3
{
    public class Match3Board
    {
        public readonly int columnCount;
        public readonly int rowCount;
        public ISlot[,] Slots;

        public Match3Board(int rowCount, int columnCont)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCont;
            
            // todo: init with level
            InitSlots();
        }
        
        private void InitSlots()
        {
            int[,] data =
            {
                {1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1},
                {0,1,1,1,1,1,1,0},
            };
            Slots = new ISlot[rowCount, columnCount];
            for (var row = 0; row < rowCount; row++)
            {
                for (var column = 0; column < columnCount; column++)
                {
                    IStateSlot state = data[row,column] == 0 ? new UnavailableSlotState() : new AvailableSlotState();
                    Slots[row, column] = new Slot(state);
                }
            }
        }

        public bool IsPointInBoard(GridPosition pos)
        {
            return (0 <= pos.ColumnIndex && pos.ColumnIndex < columnCount)
                   && (0 <= pos.RowIndex && pos.RowIndex < rowCount);
        }

        public ISlot GetSlot(GridPosition pos)
        {
            try
            {
                return Slots[pos.RowIndex, pos.ColumnIndex];
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public GridPosition GetSlotPosition(ISlot slot)
        {
            for (var row = 0; row < rowCount; row++)
            {
                for (var column = 0; column < columnCount; column++)
                {
                    if (slot == Slots[row, column]) return new GridPosition(row, column);
                }
            }

            throw new Exception("Invalid slot");
        }

    }
}
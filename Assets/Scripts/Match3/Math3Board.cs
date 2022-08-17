using Unit;
using UnityEngine;

namespace Match3
{
    public class Math3Board
    {
        public readonly int columnCount;
        public readonly int rowCount;
        public ISlot[,] Slots;

        public Math3Board(int rowCount, int columnCont)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCont;
            
            // todo: init with level
            InitSlots();
        }
        
        private void InitSlots()
        {
            Slots = new ISlot[rowCount, columnCount];
            for (var row = 0; row < rowCount; row++)
            {
                for (var column = 0; column < columnCount; column++)
                {
                    IStateSlot state = Random.Range(0f, 1f) > 0.8f ? new UnavailableSlotState() : new AvailableSlotState();
                    Slots[row, column] = new Slot(state);
                }
            }
        }
    }
}
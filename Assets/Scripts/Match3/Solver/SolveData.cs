using System.Collections.Generic;
using Unit;

namespace Match3.Solver
{
    public class SolveData
    {
        private HashSet<ISlot> SolvedSlot { get; }

        public SolveData()
        {
            // todo:
            SolvedSlot = new HashSet<ISlot>();
        }

        public void AddSlot(ISlot slot)
        {
            SolvedSlot.Add(slot);
        }

        public void AddSlot(IEnumerable<ISlot> slots)
        {
            foreach (var slot in slots)
            {
                AddSlot(slot);
            }
        }
    }
}
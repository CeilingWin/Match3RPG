using System.Collections.Generic;
using Unit;

namespace Match3.Solver
{
    public class SolveData
    {
        private List<ISlot> SolvedSlot { get; }

        public SolveData()
        {
            // todo:
            SolvedSlot = new List<ISlot>();
        }
    }
}
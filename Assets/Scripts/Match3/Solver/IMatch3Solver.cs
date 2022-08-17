using System.Collections.Generic;

namespace Match3.Solver
{
    public interface IMatch3Solver
    {
        public SolveData SolveBoard(Math3Board gameBoard, List<GridPosition> positions);
    }
}
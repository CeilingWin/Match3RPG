using System.Collections.Generic;

namespace Match3.Solver
{
    public interface IMatch3Solver
    {
        public SolveData SolveBoard(Match3Board gameBoard, List<GridPosition> positions);
    }
}
using System.Threading;
using Cysharp.Threading.Tasks;
using Match3.Solver;

namespace Match3.FillStrategies
{
    public interface IFillStrategy
    {
        public UniTask Fill(Match3Board gameBoard, CancellationToken cancellationToken);
        public UniTask Solve(Match3Board gameBoard,  SolveData solveData, CancellationToken cancellationToken);
    }
}
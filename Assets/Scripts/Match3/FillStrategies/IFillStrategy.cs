using System.Threading;
using Cysharp.Threading.Tasks;

namespace Match3.FillStrategies
{
    public interface IFillStrategy
    {
        public UniTask Fill(Math3Board gameBoard, CancellationToken cancellationToken);
        public UniTask Solve(Math3Board gameBoard, CancellationToken cancellationToken);
    }
}
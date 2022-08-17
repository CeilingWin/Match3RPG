using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Match3.FillStrategies
{
    public class FadeInStrategy : IFillStrategy
    {
        private Match3Module controller;
        public FadeInStrategy(Match3Module controller)
        {
            this.controller = controller;
        }
        public async UniTask Fill(Math3Board gameBoard, CancellationToken cancellationToken)
        {
            List<UniTask> jobs = new List<UniTask>();
            for (var rowIndex = 0; rowIndex < gameBoard.rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < gameBoard.columnCount; columnIndex++)
                {
                    Debug.Log(rowIndex + "_" + columnIndex);
                    var slot = gameBoard.Slots[rowIndex, columnIndex];
                    if (!slot.CanSetItem()) continue;
                    Debug.Log("cr it");
                    var item = ItemPool.GetIns().GetItem();
                    item.SetContentId(Random.Range(0,4));
                    item.SetPosition(controller.IndexToWorldPosition(rowIndex, columnIndex));
                    jobs.Add(item.Appear());
                }
            }
            await UniTask.WhenAll(jobs);
        }

        public UniTask Solve(Math3Board gameBoard, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
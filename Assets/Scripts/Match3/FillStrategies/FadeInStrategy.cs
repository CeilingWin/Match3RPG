using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.Solver;
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
                    var slot = gameBoard.Slots[rowIndex, columnIndex];
                    if (!slot.CanSetItem()) continue;
                    var item = ItemPool.GetIns().GetItem();
                    item.SetMaterial((Enum.Material)Random.Range(0, 4));
                    item.SetPosition(controller.IndexToWorldPosition(rowIndex, columnIndex));
                    jobs.Add(item.Appear());
                    slot.SetItem(item);
                }
            }

            await UniTask.WhenAll(jobs);
        }

        public async UniTask Solve(Math3Board gameBoard, SolveData solveData, CancellationToken cancellationToken)
        {
            List<UniTask> jobs = new List<UniTask>();
            int index = 0;
            foreach (var slot in solveData.SolvedSlot)
            {
                var itemToHide = slot.GetItem();
                // todo: do logic gen new item
                var newItem = ItemPool.GetIns().GetItem();
                newItem.SetMaterial((Enum.Material)Random.Range(0,4));
                newItem.SetPosition(itemToHide.GetPosition());
                var task1 = itemToHide.Transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
                {
                    ItemPool.GetIns().ReleaseItem(itemToHide);
                }).SetDelay(index * 0.1f).WithCancellation(cancellationToken);
                var task2 = newItem.Appear(index * 0.1f + 0.25f);
                jobs.Add(task1);
                jobs.Add(task2);
                slot.SetItem(newItem);
                // index += 1;
            }

            await UniTask.WhenAll(jobs);
        }
    }
}
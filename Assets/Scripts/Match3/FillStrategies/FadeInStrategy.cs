using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using Match3.Solver;
using UnityEngine;
using Utils;
using Material = Enum.Material;

namespace Match3.FillStrategies
{
    public class FadeInStrategy : IFillStrategy
    {
        private Match3Module controller;

        public FadeInStrategy(Match3Module controller)
        {
            this.controller = controller;
        }

        public async UniTask Fill(Match3Board gameBoard, CancellationToken cancellationToken)
        {
            List<UniTask> jobs = new List<UniTask>();
            for (var rowIndex = 0; rowIndex < gameBoard.rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < gameBoard.columnCount; columnIndex++)
                {
                    var slot = gameBoard.Slots[rowIndex, columnIndex];
                    if (!slot.CanSetItem()) continue;
                    var item = ItemPool.GetIns().GetItem();
                    item.SetMaterial(GenMaterial(gameBoard, rowIndex, columnIndex));
                    item.SetPosition(controller.IndexToWorldPosition(rowIndex, columnIndex));
                    jobs.Add(item.Appear());
                    slot.SetItem(item);
                }
            }

            await UniTask.WhenAll(jobs);
        }

        public async UniTask Solve(Match3Board gameBoard, SolveData solveData, CancellationToken cancellationToken)
        {
            List<UniTask> jobs = new List<UniTask>();
            int index = 0;
            foreach (var slot in solveData.SolvedSlot)
            {
                var itemToHide = slot.GetItem();
                // todo: do logic gen new item
                var newItem = ItemPool.GetIns().GetItem();
                var slotPos = gameBoard.GetSlotPosition(slot);
                newItem.SetMaterial(GenMaterial(gameBoard, slotPos.RowIndex, slotPos.ColumnIndex));
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

        private Enum.Material GenMaterial(Match3Board gameBoard, int rowIndex, int columnIndex)
        {
            List<Enum.Material> listMaterialToAvoid = new List<Enum.Material>();
            List<List<GridPosition>> axisToSearch = new List<List<GridPosition>>()
            {
                new() {GridPosition.Down, GridPosition.Up},
                new() {GridPosition.Left, GridPosition.Right}
            };
            var centerPos = new GridPosition(rowIndex, columnIndex);
            foreach (var axis in axisToSearch)
            {
                Dictionary<int, int> listMaterialAround = new Dictionary<int, int>();
                foreach (var dir in axis)
                {
                    int count = 0;
                    var currentPos = centerPos + dir;
                    if (!gameBoard.IsPointInBoard(currentPos)) continue;
                    if (gameBoard.GetSlot(currentPos).GetItem() == null) continue;
                    var material = gameBoard.GetSlot(currentPos).GetItem().GetMaterial();
                    do
                    {
                        var item = gameBoard.GetSlot(currentPos).GetItem();
                        if (item != null && item.GetMaterial() == material)
                        {
                            count += 1;
                            currentPos += dir;
                        }
                        else
                        {
                            break;
                        }
                    } while (gameBoard.IsPointInBoard(currentPos));

                    var key = (int) material;
                    if (count > 0)
                    {
                        if (!listMaterialAround.ContainsKey(key))
                        {
                            listMaterialAround.Add(key, 0);
                        }

                        var crrCount = listMaterialAround[key];
                        // listMaterialAround.Add(key, crrCount + count);
                        listMaterialAround[key] = crrCount + count;
                    }
                }

                foreach (var entry in listMaterialAround)
                {
                    if (entry.Value >= (GameConst.MinLengthSequence - 1))
                    {
                        listMaterialToAvoid.Add((Enum.Material)entry.Key);
                    }
                }
            }

            var arr = System.Enum.GetValues(typeof(Enum.Material)).Cast<Enum.Material>().ToArray();
            RandomUtils.ShuffleArray(arr);
            return arr.FirstOrDefault(material => !listMaterialToAvoid.Contains(material));
        }
    }
}
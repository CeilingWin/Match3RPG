using System.Threading;
using Cysharp.Threading.Tasks;
using Match3.FillStrategies;
using Unit;
using Unity.VisualScripting;
using UnityEngine;

namespace Match3
{
    public class Match3Module
    {
        private readonly float itemSize;


        private readonly GameObject itemPrefab;
        private readonly Math3Board gameBoard;
        private readonly IFillStrategy fillStrategy;

        public Match3Module(int level, GameObject itemPrefab)
        {
            // todo: load config
            this.itemPrefab = itemPrefab;
            this.itemSize = 1.1f;
            gameBoard = new Math3Board(8, 8);
            fillStrategy = new FadeInStrategy(this);
        }

        public async UniTask Init(CancellationToken cancellationToken)
        {
            Debug.Log("init m3");
            await fillStrategy.Fill(gameBoard, cancellationToken);
        }
        public Vector3 IndexToWorldPosition(int row, int column)
        {
            float z = (-gameBoard.rowCount / 2 + row + 0.5f) * itemSize;
            float x = (-gameBoard.columnCount / 2 + column + 0.5f) * itemSize;
            return new Vector3(x, 0, z);
        }

        private Item CreateNewItem(int row, int column)
        {
            var item = Object.Instantiate(itemPrefab);
            item.transform.position = IndexToWorldPosition(row, column);
            return item.GetComponent<Item>();
        }

    }
}
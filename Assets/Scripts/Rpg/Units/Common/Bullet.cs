using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3;
using UnityEngine;

namespace Rpg.Units.Common
{
    public class Bullet : MonoBehaviour
    {
        private GridPosition position;
        private float moveSpeed = 8;

        public void SetGridPosition(GridPosition value)
        {
            position = value;
            var worldPosition = Game.instance.Match3Module.IndexToWorldPosition(value);
            worldPosition.y = transform.position.y;
            transform.position = worldPosition;
        }

        public GridPosition GetGridPosition()
        {
            return position;
        }

        public async UniTask MoveTo(GridPosition gridPosition)
        {
            var desPosition = Game.instance.Match3Module.IndexToWorldPosition(gridPosition);
            desPosition.y = transform.position.y;
            var timeMove = Vector3.Distance(desPosition, transform.position) / moveSpeed;
            await transform.DOMove(desPosition, timeMove).SetEase(Ease.Linear)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
            // todo: fix bug call to here while destroyed
            SetGridPosition(gridPosition);
        }

        public static Bullet Create()
        {
            var bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"))
                .GetComponent<Bullet>();
            return bullet;
        }

        public void Hide()
        {
            Destroy(transform.gameObject);
        }
    }
}
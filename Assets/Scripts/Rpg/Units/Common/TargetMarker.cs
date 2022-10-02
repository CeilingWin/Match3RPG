using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Rpg.Units.Common
{
    public class TargetMarker : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private UniTask ShowEffect()
        {
            var seq = DOTween.Sequence();
            float t = 0.2f;
            seq.Append(spriteRenderer.DOFade(0, t));
            seq.Append(spriteRenderer.DOFade(1, t));
            seq.SetLoops(-1);
            return seq.WithCancellation(spriteRenderer.GetCancellationTokenOnDestroy());
        }

        public async UniTask Hide()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            Destroy(transform.gameObject);
        }

        public static TargetMarker CreateObject(GridPosition gridPosition)
        {
            var targetMarker = Instantiate(Resources.Load<GameObject>("Prefabs/TargetMarker"))
                .GetComponent<TargetMarker>();
            targetMarker.ShowEffect();
            var pos = Game.instance.Match3Module.IndexToWorldPosition(gridPosition);
            pos.y = 0.01f;
            targetMarker.transform.position = pos;
            return targetMarker;
        }
    }
}
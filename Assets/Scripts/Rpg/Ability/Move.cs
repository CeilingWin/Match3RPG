using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Rpg.Ability
{
    public class Move : MonoBehaviour
    {
        private const float MoveSpeed = 1.5f; // unit/s
        private const float RotationSpeed = 60f;
        private Animator animator;

        private readonly int walkAction = Animator.StringToHash("Walk");
        private readonly int idleAction = Animator.StringToHash("Idle");
        private void OnEnable()
        {
            animator = GetComponent<Animator>();
        }

        public async UniTask MoveTo(GridPosition gridPosition)
        {
            var desPosition = Game.instance.Match3Module.IndexToWorldPosition(gridPosition);
            var position = transform.position;
            var timeMove = Vector3.Distance(desPosition, position) / MoveSpeed;
            animator.Play(walkAction);
            var rotationJob = DoActionRotateTo(transform.forward, desPosition - position);
            var moveJob = transform.DOMove(desPosition, timeMove).WithCancellation(this.GetCancellationTokenOnDestroy());
            await UniTask.WhenAll(moveJob, rotationJob);
            var rotateBackJob = DoActionRotateTo(desPosition - position, Vector3.forward);
            animator.Play(idleAction);
            await rotateBackJob;
            // todo: fix move action
        }

        private UniTask DoActionRotateTo(Vector3 currentDirection, Vector3 direction)
        {
            var desRotation = Quaternion.LookRotation(direction - currentDirection);
            var currentRotation = Quaternion.Euler(currentDirection);
            var timeRotation = Quaternion.Angle(desRotation, currentRotation) / RotationSpeed;
            return transform.DORotateQuaternion(desRotation, timeRotation)
                .SetEase(Ease.OutBack)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
        }
    }
}
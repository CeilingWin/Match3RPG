﻿using System;
using System.Collections.Generic;
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
        private readonly int walkSlowAnim = Animator.StringToHash("WalkSlow");
        private void OnEnable()
        {
            animator = GetComponent<Animator>();
        }

        public async UniTask MoveTo(GridPosition gridPosition, bool changeAnimation = true)
        {
            var desPosition = Game.instance.Match3Module.IndexToWorldPosition(gridPosition);
            var position = transform.position;
            var timeMove = Vector3.Distance(desPosition, position) / MoveSpeed;
            if (changeAnimation) animator.Play(walkAction);
            var rotationJob = DoActionRotateTo(transform.forward, desPosition - position);
            var moveJob = transform.DOMove(desPosition, timeMove).SetEase(Ease.Linear).WithCancellation(this.GetCancellationTokenOnDestroy());
            await UniTask.WhenAll(moveJob, rotationJob);
            if (changeAnimation) animator.Play(idleAction);
            var rotateBackJob = DoActionRotateTo(desPosition - position, GetComponent<Units.Unit>().defaultDirection);
            await rotateBackJob;
            GetComponent<Units.Unit>()?.SetGridPosition(gridPosition);
        }

        public async UniTask MoveUsingPath(List<GridPosition> path, int numMoveAvailable)
        {
            if (path == null) return;
            animator.Play(walkAction);
            foreach (var nextPosition in path)
            {
                if (numMoveAvailable == 0) break;
                await MoveTo(nextPosition, false);
                numMoveAvailable--;
            }
            animator.Play(idleAction);
        }

        private UniTask DoActionRotateTo(Vector3 currentDirection, Vector3 direction)
        {
            // var desRotation = Quaternion.LookRotation(direction - currentDirection);
            // var currentRotation = Quaternion.Euler(currentDirection);
            // var timeRotation = Quaternion.Angle(desRotation, currentRotation) / RotationSpeed;
            // return transform.DORotateQuaternion(desRotation, 0)
            //     .SetEase(Ease.OutBack)
            //     .WithCancellation(this.GetCancellationTokenOnDestroy());
            transform.rotation = Quaternion.LookRotation(direction);
            return UniTask.CompletedTask;
        }

        public UniTask RotateToMonsterSpawner()
        {
            return DoActionRotateTo(transform.forward, Vector3.forward);
        }
    }
}
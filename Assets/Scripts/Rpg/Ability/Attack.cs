using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Rpg.Ability
{
    public class Attack : MonoBehaviour
    {
        private Animator animator;
        private readonly int attackAction = Animator.StringToHash("Attack1");
        private void OnEnable()
        {
            animator = GetComponent<Animator>();
        }

        public async UniTask DoAttack()
        {
            animator.Play(attackAction);
            var idleAction = Animator.StringToHash("Idle");
            await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == idleAction);
        }
    }
}
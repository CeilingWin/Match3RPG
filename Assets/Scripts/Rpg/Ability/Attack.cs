using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

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
            var idleAction = Animator.StringToHash("Idle");
            await AnimatorUtils.PlayAnimationSync(animator, attackAction, idleAction);
        }
    }
}
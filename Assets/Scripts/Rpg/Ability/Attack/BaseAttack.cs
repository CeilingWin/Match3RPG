using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Rpg.Ability
{
    public abstract class BaseAttack : MonoBehaviour
    {
        private Animator animator;
        private readonly int attackAction = Animator.StringToHash("Attack1");
        
        private void OnEnable()
        {
            animator = GetComponent<Animator>();
        }

        protected async UniTask DoAttack()
        {
            var idleAction = Animator.StringToHash("Idle");
            await AnimatorUtils.PlayAnimationSync(animator, attackAction, idleAction);
        }
    }
}
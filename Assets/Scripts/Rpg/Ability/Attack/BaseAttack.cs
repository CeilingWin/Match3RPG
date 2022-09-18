using Cysharp.Threading.Tasks;
using Rpg.Effects;
using UnityEngine;
using Utils;

namespace Rpg.Ability
{
    public abstract class BaseAttack : MonoBehaviour
    {
        private Animator animator;
        private readonly int attackAction = Animator.StringToHash("Attack1");
        private Effect effect;

        public void SetEffect(Effect value)
        {
            effect = value;
        }

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
        }

        public async UniTask DoAttack()
        {
            var idleAction = Animator.StringToHash("Idle");
            await AnimatorUtils.PlayAnimationSync(animator, attackAction, idleAction);
        }

        protected Effect GetAttackEffect()
        {
            return effect?.Clone();
        }

        protected void PerformAttackToTarget(Units.Unit target)
        {
            target.TakeDamage(GetComponent<Stat>().GetDamage());
            var attackEffect = GetAttackEffect();
            if (attackEffect != null) target.TakeEffect(attackEffect);
        }
    }
}
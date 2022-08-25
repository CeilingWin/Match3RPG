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

        public void DoAttack()
        {
            animator.Play(attackAction);
        }
    }
}
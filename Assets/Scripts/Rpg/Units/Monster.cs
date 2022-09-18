using Cysharp.Threading.Tasks;
using Rpg.Ability;
using UnityEngine;
using Utils;

namespace Rpg.Units
{
    public abstract class Monster : Unit
    {
        protected override void Start()
        {
            base.Start();
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        }

        public override UniTask Attack()
        {
            throw new System.NotImplementedException();
        }

        public override async UniTask Die()
        {
            var animator = GetComponent<Animator>();
            await AnimatorUtils.PlayAnimationSync(animator, "Die", "Idle");
            Destroy(gameObject);
        }

        public override bool IsDied()
        {
            return GetComponent<Stat>().GetHp() > 0;
        }

        public override async UniTask TakeDamage(int damage)
        {
            var stat = GetComponent<Stat>();
            stat.ChangeHp(-damage);
            if (stat.GetHp() == 0)
            {
                Game.instance.RpgModule.OnMonsterDied(this);
                await Die();
            }
            else
            {
                var animator = GetComponent<Animator>();
                await AnimatorUtils.PlayAnimationSync(animator, "GetHit", "Idle");
            }
        }
    }
}
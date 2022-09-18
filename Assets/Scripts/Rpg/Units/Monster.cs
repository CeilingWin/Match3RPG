using Cysharp.Threading.Tasks;
using Rpg.Ability;
using UnityEngine;

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

        public override UniTask Die()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsDied()
        {
            return GetComponent<Stat>().GetHp() > 0;
        }
    }
}
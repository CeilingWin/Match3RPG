using Cysharp.Threading.Tasks;

namespace Rpg.Units
{
    public abstract class Monster : Unit
    {
        new void Start()
        {
            base.Start();
        }
        
        public override UniTask Attack()
        {
            throw new System.NotImplementedException();
        }

        public override UniTask Die()
        {
            throw new System.NotImplementedException();
        }
    }
}
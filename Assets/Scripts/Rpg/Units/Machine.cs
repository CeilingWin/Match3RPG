using Cysharp.Threading.Tasks;
using Rpg.Ability;
using Unity.VisualScripting;

namespace Rpg.Units
{
    public class Machine : Unit
    {
        new void Start()
        {
            base.Start();
            this.AddComponent<Attack>();
        }

        public override UniTask Attack()
        {
            throw new System.NotImplementedException();
        }
    }
}
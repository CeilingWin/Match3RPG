using Rpg.Ability;
using Unity.VisualScripting;

namespace Rpg.Units.Machines
{
    public class Paralyzer : Machine
    {
        protected override void Start()
        {
            base.Start();
            this.AddComponent<SquareDetect>();
            GetComponent<Stat>().SetStat(3, 1, 2, 1, 5);
        }
    }
}
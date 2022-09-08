using Rpg.Ability;
using Unity.VisualScripting;

namespace Rpg.Units.Machines
{
    public class Brawler : Machine
    {
        protected override void Start()
        {
            base.Start();
            this.AddComponent<SquareDetect>();
            GetComponent<Stat>().SetStat(5, 1, 1, 2, 3);
        }
    }
}
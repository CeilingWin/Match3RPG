using Rpg.Ability;
using Rpg.Ability.Detection;
using Unity.VisualScripting;

namespace Rpg.Units.Machines
{
    public class Brawler : Machine
    {
        protected override void Start()
        {
            base.Start();
            this.AddComponent<SquareDetection>();
            GetComponent<Stat>().SetStat(5, 1, 1, 2, 3);
        }
    }
}
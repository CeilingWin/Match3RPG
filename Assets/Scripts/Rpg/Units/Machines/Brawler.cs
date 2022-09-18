using Enum;
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
            material = Material.Machinery;
            this.AddComponent<SquareDetection>();
            GetComponent<Stat>().SetStat(5, 1, 1, 2, 3);
            this.AddComponent<SingleTargetAttack>();
        }
    }
}
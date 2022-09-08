using Rpg.Ability;

namespace Rpg.Units.Machines
{
    public class Paralyzer : Machine
    {
        protected override void Start()
        {
            base.Start();
            GetComponent<Stat>().SetStat(3, 1, 2, 1, 5);
        }
    }
}
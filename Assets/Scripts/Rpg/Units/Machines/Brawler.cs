using Rpg.Ability;

namespace Rpg.Units.Machines
{
    public class Brawler : Machine
    {
        new void Start()
        {
            base.Start();
            GetComponent<Stat>().SetStat(5, 1, 1, 2, 3);
        }
    }
}
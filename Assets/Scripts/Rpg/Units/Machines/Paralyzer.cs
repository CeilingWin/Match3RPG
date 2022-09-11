using Rpg.Ability;
using Rpg.Ability.Detection;
using Unity.VisualScripting;

namespace Rpg.Units.Machines
{
    public class Paralyzer : Machine
    {
        protected override void Start()
        {
            base.Start();
            var detection = this.AddComponent<RangeDetection>();
            detection.SetRange(3);
            GetComponent<Stat>().SetStat(3, 1, 2, 1, 5);
        }
    }
}
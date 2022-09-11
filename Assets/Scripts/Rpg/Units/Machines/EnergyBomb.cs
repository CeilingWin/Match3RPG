using System;
using Rpg.Ability;
using Rpg.Ability.Detection;
using Unity.VisualScripting;

namespace Rpg.Units.Machines
{
    public class EnergyBomb : Machine
    {
        protected override void Start()
        {
            base.Start();
            var detection = this.AddComponent<RangeDetection>();
            detection.SetRange(3);
            GetComponent<Stat>().SetStat(1, 0, Int32.MaxValue, 2, 2);
        }
    }
}
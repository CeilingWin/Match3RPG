using System;
using Rpg.Ability;
using Unity.VisualScripting;

namespace Rpg.Units.Machines
{
    public class EnergyBomb : Machine
    {
        protected override void Start()
        {
            base.Start();
            this.AddComponent<SquareDetect>();
            GetComponent<Stat>().SetStat(1, 0, Int32.MaxValue, 2, 2);
        }
    }
}
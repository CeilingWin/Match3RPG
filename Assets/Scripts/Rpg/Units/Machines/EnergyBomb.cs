﻿using System;
using Rpg.Ability;

namespace Rpg.Units.Machines
{
    public class EnergyBomb : Machine
    {
        protected override void Start()
        {
            base.Start();
            GetComponent<Stat>().SetStat(1, 0, Int32.MaxValue, 2, 2);
        }
    }
}
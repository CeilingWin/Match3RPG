using System;
using Rpg.Ability;

namespace Rpg.Units.Monsters
{
    public class Shark : Monster
    {
        new void Start()
        {
            base.Start();
            GetComponent<Stat>().SetStat(3, 4, 1, 3, Int32.MaxValue);
        }
    }
}
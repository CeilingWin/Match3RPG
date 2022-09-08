using System;
using Rpg.Ability;

namespace Rpg.Units.Monsters
{
    public class Wasp : Monster
    {
        new void Start()
        {
            base.Start();
            GetComponent<Stat>().SetStat(3, 2, 1, 3, Int32.MaxValue);
        }
    }
}
using System;
using Cysharp.Threading.Tasks;
using Rpg.Ability;

namespace Rpg.Units.Monsters
{
    public class Bulldog : Monster
    {
        new void Start()
        {
            base.Start();
            GetComponent<Stat>().SetStat(6, 1, 1, 6, Int32.MaxValue);
        }
    }
}
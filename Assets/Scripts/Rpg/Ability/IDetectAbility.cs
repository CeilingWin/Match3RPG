using System.Collections.Generic;
using Match3;

namespace Rpg.Ability
{
    public interface IDetectAbility
    {
        public List<Units.Unit> DetectAllUnits<T>() where T : Units.Unit;
    }
}
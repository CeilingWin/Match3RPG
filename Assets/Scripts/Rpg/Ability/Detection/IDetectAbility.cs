using System.Collections.Generic;

namespace Rpg.Ability.Detection
{
    public interface IDetectAbility
    {
        public List<Units.Unit> DetectAllUnits<T>() where T : Units.Unit;
        public int GetRange();
    }
}
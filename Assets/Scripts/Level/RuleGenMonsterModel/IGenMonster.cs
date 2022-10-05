using System.Collections.Generic;
using Enum;

namespace Level.RuleGenMonsterModel
{
    public interface IGenMonster
    {
        public Dictionary<MonsterType, int> GetGenerateMonsters(int round, int currentNumMonsters);
    }
}
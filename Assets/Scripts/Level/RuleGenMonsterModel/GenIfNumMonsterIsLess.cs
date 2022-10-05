using System.Collections.Generic;
using Enum;

namespace Level.RuleGenMonsterModel
{
    public class GenIfNumMonsterIsLess : IGenMonster
    {
        private readonly int numMonsterPerRound;
        private readonly int numMonsterRequire;
        private readonly MonsterType monsterType;

        public GenIfNumMonsterIsLess(int numMonsterPerRound, int numMonsterRequire, MonsterType monsterType)
        {
            this.numMonsterPerRound = numMonsterPerRound;
            this.numMonsterRequire = numMonsterRequire;
            this.monsterType = monsterType;
        }
        public Dictionary<MonsterType, int> GetGenerateMonsters(int round, int currentNumMonsters)
        {
            Dictionary<MonsterType, int> result = new Dictionary<MonsterType, int>();
            if (currentNumMonsters < numMonsterRequire)
            {
                result.Add(monsterType, numMonsterPerRound);
            }
            return result;
        }
    }
}
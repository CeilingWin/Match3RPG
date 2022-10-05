using System.Collections.Generic;
using Enum;

namespace Level.RuleGenMonsterModel
{
    public class GenIfRoudEquals : IGenMonster
    {
        private readonly int numMonsterPerRound;
        private readonly int roundNumberRequire;
        private readonly MonsterType monsterType;

        public GenIfRoudEquals(int numMonsterPerRound, int roundNumberRequire, MonsterType monsterType)
        {
            this.numMonsterPerRound = numMonsterPerRound;
            this.roundNumberRequire = roundNumberRequire;
            this.monsterType = monsterType;
        }
        public Dictionary<MonsterType, int> GetGenerateMonsters(int round, int currentNumMonsters)
        {
            Dictionary<MonsterType, int> result = new Dictionary<MonsterType, int>();
            if (round == roundNumberRequire)
            {
                result.Add(monsterType, numMonsterPerRound);
            }
            return result;
        }
    }
}
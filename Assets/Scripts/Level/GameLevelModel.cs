using System.Collections.Generic;
using Enum;
using Level.RuleGenMonsterModel;
using Unity.VisualScripting;

namespace Level
{
    public class WaveConfig
    {
        public readonly Dictionary<MonsterType, int> totalMonster;
        public readonly List<IGenMonster> rules;

        public WaveConfig()
        {
            totalMonster = new Dictionary<MonsterType, int>();
            rules = new List<IGenMonster>();
        }

        public WaveConfig AddMonster(MonsterType monsterType, int quantity)
        {
            if (!totalMonster.ContainsKey(monsterType))
            {
                totalMonster.Add(monsterType, 0);
            }

            quantity += totalMonster[monsterType];
            totalMonster[monsterType] = quantity;
            return this;
        }

        public WaveConfig AddRule(IGenMonster rule)
        {
            rules.Add(rule);
            return this;
        }
    }
    public class GameLevelModel
    {
        public readonly int playerHealth;
        public readonly List<WaveConfig> waveConfigs;

        public GameLevelModel(int playerHealth)
        {
            this.playerHealth = playerHealth;
            waveConfigs = new List<WaveConfig>();
        }

        public GameLevelModel AddWave(WaveConfig waveConfig)
        {
            waveConfigs.Add(waveConfig);
            return this;
        }

        public WaveConfig GetWaveConfig(int wave)
        {
            return waveConfigs[wave-1];
        }

        public int GetNumWave()
        {
            return waveConfigs.Count;
        }
    }
}
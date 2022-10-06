using System;
using System.Collections.Generic;
using DefaultNamespace;
using Enum;
using Level.RuleGenMonsterModel;

namespace Level
{
    public class LevelConfig
    {
        public static LevelConfig instance = new LevelConfig();
        private List<GameLevelModel> gameLevels;

        private LevelConfig()
        {
            const int playerHealth = 20;
            gameLevels = new List<GameLevelModel>();
            // level 1
            var wave1 = new WaveConfig()
                .AddMonster(MonsterType.Bulldog, 4)
                .AddRule(new GenIfNumMonsterIsLess(1, 5, MonsterType.Bulldog));
            var wave2 = new WaveConfig()
                .AddMonster(MonsterType.Bulldog, 4)
                .AddMonster(MonsterType.Shark, 2)
                .AddRule(new GenIfNumMonsterIsLess(1, 5, MonsterType.Bulldog))
                .AddRule(new GenIfRoudEquals(1, 2, MonsterType.Shark))
                .AddRule(new GenIfRoudEquals(1, 4, MonsterType.Shark));
            var level1 = new GameLevelModel(playerHealth).AddWave(wave1).AddWave(wave2);
            // level 2
            wave1 = new WaveConfig()
                .AddMonster(MonsterType.Bulldog, 5)
                .AddMonster(MonsterType.Wasp, 3)
                .AddRule(new GenIfNumMonsterIsLess(1, 5, MonsterType.Bulldog))
                .AddRule(new GenIfRoundIsMore(1, 2, MonsterType.Wasp));
            wave2 = new WaveConfig()
                .AddMonster(MonsterType.Bulldog, 2)
                .AddMonster(MonsterType.Shark, 3)
                .AddMonster(MonsterType.Wasp, 5)
                .AddRule(new GenIfRoundIsMore(1, 0, MonsterType.Bulldog))
                .AddRule(new GenIfRoudEquals(1, 2, MonsterType.Shark))
                .AddRule(new GenIfRoundIsMore(1, 0, MonsterType.Wasp));
            var level2 = new GameLevelModel(playerHealth).AddWave(wave1).AddWave(wave2);
            // level 3
            wave1 = new WaveConfig()
                .AddMonster(MonsterType.Bulldog, 6)
                .AddMonster(MonsterType.Octopus, 5)
                .AddRule(new GenIfNumMonsterIsLess(1, 5, MonsterType.Bulldog))
                .AddRule(new GenIfRoundIsMore(1, 0, MonsterType.Octopus));
            wave2 = new WaveConfig()
                .AddMonster(MonsterType.Octopus, 10)
                .AddRule(new GenIfNumMonsterIsLess(2, 7, MonsterType.Octopus));
            var level3 = new GameLevelModel(playerHealth).AddWave(wave1).AddWave(wave2);
            gameLevels.Add(level1);
            gameLevels.Add(level2);
            gameLevels.Add(level3);
        }

        public int GetNumLevel()
        {
            return 3;
        }

        public GameLevelModel GetGameLevelConfig()
        {
            var currentLevel = GameConfig.currentLevel;
            return gameLevels[currentLevel - 1];
        }

        public GameLevelModel GetGameLevelConfig(int level)
        {
            try
            {
                return gameLevels[level];
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
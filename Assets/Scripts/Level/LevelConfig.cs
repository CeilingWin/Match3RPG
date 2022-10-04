namespace Level
{
    public class LevelConfig
    {
        public static LevelConfig instance = new LevelConfig();
        private LevelConfig()
        {
            // todo:
        }
        public int GetNumLevel()
        {
            return 3;
        }
    }
}
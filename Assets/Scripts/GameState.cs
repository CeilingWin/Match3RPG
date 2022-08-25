namespace DefaultNamespace
{
    public enum GamePhase
    {
        GenerateMonster = 0,
        PlayerMove = 1,
        MachineAttack = 2,
        MonsterAttack = 3,
        Ended = 4
    }
    public class GameState
    {
        private int currentTurn;
        private int wave;
        private GamePhase gamePhase;
        private int numberMoveRemain;

        public GameState()
        {
            // todo: init by level
            currentTurn = 0;
            wave = 1;
            gamePhase = GamePhase.GenerateMonster;
            numberMoveRemain = 0;
        }

        public void SetPhase(GamePhase phase)
        {
            if (phase == GamePhase.PlayerMove)
            {
                numberMoveRemain = GameConst.NumPlayerMove;
            }
            gamePhase = phase;
        }

        public GamePhase GetCurrentPhase()
        {
            return gamePhase;
        }

        public void DecreaseNumMove()
        {
            numberMoveRemain--;
        }

        public int GetNumberMoveRemain()
        {
            return numberMoveRemain;
        }

        public int GetWave()
        {
            return wave;
        }

        public void IncreaseWave()
        {
            wave++;
        }
    }
}
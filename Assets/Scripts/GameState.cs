namespace DefaultNamespace
{
    public enum GamePhase
    {
        BeginNewWave = 0,
        GenerateMonster = 1,
        PlayerMove = 2,
        MachineAttack = 3,
        MonsterAttack = 4,
        Ended = 5
    }
    public class GameState
    {
        private int wave;
        private int currentTurn;
        private GamePhase gamePhase;
        private int numberMoveRemain;

        public GameState()
        {
            // todo: init by level
            currentTurn = 1;
            wave = 0;
            gamePhase = GamePhase.BeginNewWave;
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
            currentTurn = 1;
        }

        public int GetCurrentTurn()
        {
            return currentTurn;
        }

        public void NextTurn()
        {
            currentTurn++;
        }
    }
}
namespace Rpg.Effects
{
    public abstract class Effect
    {
        protected int countDownToRemove;
        protected Units.Unit target;

        public bool isActive
        {
            get;
            set;
        }

        public void SetTarget(Units.Unit unit)
        {
            target = unit;
            isActive = true;
        }

        public void SetCountDownToRemove(int countDown)
        {
            countDownToRemove = countDown;
        }

        public void Update()
        {
            if (!this.isActive) return;
            countDownToRemove--;
            if (countDownToRemove == 0)
            {
                isActive = false;
                Destroy();
            }
        }
        public abstract void Perform();
        public abstract void Destroy();

        public abstract Effect Clone();
    }
}
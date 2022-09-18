using Enum;
using Rpg.Ability;

namespace Rpg.Effects
{
    public class ChangeSpeedEffect : Effect
    {
        public EffectType type = EffectType.ChangeSpeed;
        private readonly int amount;
        private int amountIncreased;

        public ChangeSpeedEffect(int amount, int during)
        {
            SetCountDownToRemove(during);
            this.amount = amount;
        }

        public override void Perform()
        {
            var targetStat = target.GetComponent<Stat>();
            var oldSpeed = targetStat.GetSpeed();
            targetStat.IncreaseSpeed(amount);
            amountIncreased = targetStat.GetSpeed() - oldSpeed;
        }

        public override void Destroy()
        {
            var targetStat = target.GetComponent<Stat>();
            targetStat.IncreaseSpeed(-amountIncreased);
        }

        public override Effect Clone()
        {
            return new ChangeSpeedEffect(amount, countDownToRemove);
        }
    }
}
using Rpg.Units.Common;
using Unity.VisualScripting;
using UnityEngine;

namespace Rpg.Ability
{
    public class Stat : MonoBehaviour
    {
        private int hp;
        private int speed;
        private int numTurnToAttack;
        private int damage;
        private int countDown;

        public void SetStat(int hp, int speed, int numTurnToAttack, int damage, int countDown)
        {
            this.hp = hp;
            this.speed = speed;
            this.numTurnToAttack = numTurnToAttack;
            this.damage = damage;
            this.countDown = countDown;
            GetComponentInChildren<HealthBar>().SetMaxHealth(hp);
            GetComponentInChildren<HealthBarMachine>()?.SetCountDown(countDown);
        }

        public int GetHp()
        {
            return hp;
        }

        public void ChangeHp(int hp)
        {
            this.hp += hp;
            if (this.hp < 0) this.hp = 0;
            GetComponentInChildren<HealthBar>().SetHealth(this.hp);
        }

        public int GetSpeed()
        {
            return speed;
        }

        public void ChangeSpeed(int speed)
        {
            this.speed += speed;
            if (this.speed < 1) this.speed = 1;
        }

        public int GetDamage()
        {
            return damage;
        }

        public int GetCountDown()
        {
            return countDown;
        }

        public void ChangeCountDown(int amount)
        {
            countDown += amount;
            if (countDown < 0) countDown = 0;
            GetComponentInChildren<HealthBarMachine>()?.SetCountDown(countDown);
        }
    }
}
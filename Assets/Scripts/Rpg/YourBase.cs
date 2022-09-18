using UnityEngine;

namespace Rpg
{
    public class YourBase : MonoBehaviour
    {
        private int maxHp;
        private int currentHp;

        public void SetMaxHp(int maxHp)
        {
            this.maxHp = maxHp;
            currentHp = maxHp;
        }

        public void SetHp(int hp)
        {
            if (hp < 0) hp = 0;
            if (hp > maxHp) hp = maxHp;
            this.currentHp = hp;
            float scale = 1f * currentHp / maxHp;
            transform.localScale = new Vector3(scale, 1, 1);
            transform.localPosition = new Vector3((scale - 1) / 2, 0, 0);
        }

        public void TakeDamage(int damage)
        {
            SetHp(this.currentHp - damage);
        }
    }
}
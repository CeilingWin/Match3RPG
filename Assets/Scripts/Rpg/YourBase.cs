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
        }

        public void SetHp(int hp)
        {
            this.currentHp = hp;

            float scale = 1f * currentHp / maxHp;
            if (scale > 1) scale = 1;
            if (scale < 0) scale = 0;
            transform.localScale = new Vector3(scale, 1, 1);
            transform.localPosition = new Vector3((scale - 1) / 2, 0, 0);
        }
    }
}
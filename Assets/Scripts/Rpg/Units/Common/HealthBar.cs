using UnityEngine;
using UnityEngine.UI;

namespace Rpg.Units.Common
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] protected Slider slider;
        [SerializeField] protected Image fill;

        [SerializeField] private Gradient gradient;

        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }

        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            SetHealth(health);
        }

        public void SetHealth(int health)
        {
            slider.value = health;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}

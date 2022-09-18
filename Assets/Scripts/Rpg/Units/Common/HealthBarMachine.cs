using UnityEngine;
using UnityEngine.UI;

namespace Rpg.Units.Common
{
    public class HealthBarMachine : HealthBar
    {
        [SerializeField] private Text textCountDown;

        public void SetCountDown(int countDown)
        {
            textCountDown.text = countDown.ToString();
        }
    }
}
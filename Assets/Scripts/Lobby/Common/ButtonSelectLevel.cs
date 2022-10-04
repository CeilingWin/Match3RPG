using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Lobby.Common
{
    public class ButtonSelectLevel : MonoBehaviour
    {
        [SerializeField] private Button button;

        [SerializeField] private Text text;

        public void SetString(string str)
        {
            text.text = str;
        }

        public void SetCallFunc(UnityAction func)
        {
            button.onClick.AddListener(func);
        }

        public static ButtonSelectLevel Create(Transform parent)
        {
            var btn = Instantiate(Resources.Load<GameObject>("Prefabs/Lobby/ButtonSelectLevel"))
                .GetComponent<ButtonSelectLevel>();
            btn.GetComponent<RectTransform>().SetParent(parent);
            return btn;
        }
    }
}

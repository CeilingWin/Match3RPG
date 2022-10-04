using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Popup
{
    public class PopupGameLose : MonoBehaviour
    {
        public void Start()
        {
            Time.timeScale = 0;
        }

        public void onButtonReplayClick()
        {
            SceneManager.LoadScene("GameScene");
            ResumeGame();
        }

        public void onButtonBackClick()
        {
            SceneManager.LoadScene("SceneLobby");
            ResumeGame();
        }

        public static PopupGameLose Create()
        {
            return GameObjectUtils.CreateObject<PopupGameLose>("Prefabs/Popup/PopupLoseGame");
        }

        private void ResumeGame()
        {
            Time.timeScale = 1;
        }
    }
}

using DefaultNamespace;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utils;

namespace Popup
{
    public class PopupGameWin : PopupGameLose
    {
        [SerializeField] private GameObject buttonNextLevel;
        public override void Start()
        {
            base.Start();
            if (GameConfig.currentLevel >= LevelConfig.instance.GetNumLevel())
            {
                buttonNextLevel.SetActive(false);
            }
        }

        public void onButtonNextLevelClick()
        {
            GameConfig.currentLevel++;
            SceneManager.LoadScene("GameScene");
            ResumeGame();
        }
        
        public new static PopupGameWin Create()
        {
            return GameObjectUtils.CreateObject<PopupGameWin>("Prefabs/Popup/PopupWinGame");
        }
    }
}
using DefaultNamespace;
using Level;
using Lobby.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lobby
{
    public class SceneLobby : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        void Start()
        {
            var numLevel = LevelConfig.instance.GetNumLevel();
            var crrY = 150;
            for (var i = 0; i < numLevel; i++)
            {
                var level = i + 1;
                var btn = ButtonSelectLevel.Create(canvas.transform);
                var pos = new Vector3(0, crrY, 0);
                btn.GetComponent<RectTransform>().localPosition = pos;
                btn.SetString("Level " + level);
                btn.SetCallFunc(() =>
                {
                    GameConfig.currentLevel = level;
                    OpenGame();
                });
                crrY -= 140;
            }
        }

        public void OpenGame()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Text TextWave;
    [SerializeField] private Text TextLevel;
    [SerializeField] private Text TextNotification;
    void Start()
    {
        TextLevel.text = "Level " + GameConfig.currentLevel;
        SetTextOpacity(TextNotification, 0);
    }

    public void UpdateStates()
    {
        var state = Game.instance.GetState();
        var currentWave = state.GetWave();
        var maxWave = Game.instance.GetNumberWave();
        TextWave.text = "Wave: " + currentWave + "/" + maxWave;
    }

    public void BackToLobby()
    {
        Game.instance.Destroy();
        SceneManager.LoadScene("SceneLobby");
    }

    public void ShowNotification(string notification, float during = 1)
    {
        TextNotification.text = notification;
        var seq = DOTween.Sequence();
        SetTextOpacity(TextNotification, 0);
        seq.Append(TextNotification.DOFade(1, 1));
        seq.AppendInterval(during);
        seq.Append(TextNotification.DOFade(0, 2));
    }

    private void SetTextOpacity(Text text, float opacity)
    {
        var currentColor = text.color;
        currentColor.a = 0;
        text.color = currentColor;
    }
}

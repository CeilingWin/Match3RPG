using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Text TextWave;
    [SerializeField] private Text TextLevel;
    // Start is called before the first frame update
    void Start()
    {
        TextLevel.text = "Level " + GameConfig.currentLevel;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStates();
    }

    private void UpdateStates()
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
}

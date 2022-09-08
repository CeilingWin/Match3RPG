using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Text TextWave;
    // Start is called before the first frame update
    void Start()
    {
        
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
}

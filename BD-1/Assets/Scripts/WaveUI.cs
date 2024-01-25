using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    private TextMeshProUGUI waveNumberUI;
    private TextMeshProUGUI nextWaveTimerUI;


    void Start()
    {
        waveNumberUI = this.transform.Find("WaveNumber").GetComponent<TextMeshProUGUI>();
        nextWaveTimerUI = this.transform.Find("NextWave").GetComponent<TextMeshProUGUI>();

        EnemyWaveManager.Instance.OnWaveChange += EWM_OnWaveChange;
        EnemyWaveManager.Instance.OnNextWaveTimeChange += EWM_OnNextWaveTimeChange;
    }

    private void EWM_OnNextWaveTimeChange(object sender, float e)
    {
        nextWaveTimerUI.text = "Next Wave " + e.ToString("F1");
    }

    private void EWM_OnWaveChange(object sender, int e)
    {
        waveNumberUI.text = "Wave " + e.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

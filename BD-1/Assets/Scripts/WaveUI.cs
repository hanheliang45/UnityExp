using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    private TextMeshProUGUI waveNumberUI;
    private TextMeshProUGUI nextWaveTimerUI;
    private RectTransform waveArrow;


    void Start()
    {
        waveNumberUI = this.transform.Find("WaveNumber").GetComponent<TextMeshProUGUI>();
        nextWaveTimerUI = this.transform.Find("NextWave").GetComponent<TextMeshProUGUI>();
        waveArrow = this.transform.Find("WaveArrow").GetComponent<RectTransform>(); ;

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
        Vector3 arrowDirection = (EnemyWaveManager.Instance.GetSpawnPosition()
            - Camera.main.transform.position).normalized;
        waveArrow.eulerAngles = new Vector3(0, 0,
            Tools.Direction2Degree(arrowDirection)
        );
        waveArrow.anchoredPosition = Camera.main.transform.position + 200 * arrowDirection;
    }
}

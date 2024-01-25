using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance;


    [SerializeField] List<Transform> positionList;
    [SerializeField] Transform spawnCircle;

    public event EventHandler<int> OnWaveChange;
    public event EventHandler<float> OnNextWaveTimeChange;

    private EnemyWaveState state;

    private int enemyToSpawn;
    private float timer_next_wave;
    private float timer_next_wave_max = 5f;
    private float timer_next_enemy;

    private Vector3 spawnPosition;
    private int waveNumber = 0;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        state = EnemyWaveState.WAITING_NEXT_WAVE;
        timer_next_wave = timer_next_wave_max;
        timer_next_enemy = 0;

        spawnPosition = positionList[UnityEngine.Random.Range(0, positionList.Count)].position;
        spawnCircle.position = spawnPosition;

        waveNumber++;
        OnWaveChange?.Invoke(this, waveNumber);
    }

    void Update()
    {
        switch (state)
        {
            case EnemyWaveState.WAITING_NEXT_WAVE:
                
                timer_next_wave -= Time.deltaTime;
                OnNextWaveTimeChange?.Invoke(this, timer_next_wave);

                if (timer_next_wave <= 0)
                {
                    state = EnemyWaveState.WAITING_SPAWN_ENEMY;
                    enemyToSpawn = waveNumber * 3 + 5;
                    
                }
                break;
            case EnemyWaveState.WAITING_SPAWN_ENEMY:
                if (enemyToSpawn == 0)
                {
                    state = EnemyWaveState.WAITING_NEXT_WAVE;
                    timer_next_wave = timer_next_wave_max;

                    spawnPosition = positionList[UnityEngine.Random.Range(0, positionList.Count)].position;
                    spawnCircle.position = spawnPosition;

                    waveNumber++;
                    OnWaveChange?.Invoke(this, waveNumber);
                }
                else
                {
                    timer_next_enemy -= Time.deltaTime;
                    if (timer_next_enemy <= 0)
                    {
                        timer_next_enemy = UnityEngine.Random.Range(0, 0.3f);

                        Enemy enemy = Enemy.Create(spawnPosition + 10 * Tools.GetRandomeDirection());
                        enemy.GetComponent<HealthSystem>().SetMaxHP(20);

                        enemyToSpawn--;
                    }
                                    
                }
                
                break;
        }
        
    }
}

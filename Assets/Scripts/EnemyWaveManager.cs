using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance { get; private set; }

    public event System.EventHandler OnWaveNumberChanged;

    private enum State
    {
        WaitingToNextWave,
        SpawningWave,
    }

    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPosition;

    private State state;
    private int waveNumber;

    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;

    private Vector3 spawnPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        state = State.WaitingToNextWave;
        spawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPosition.position = spawnPosition;
        nextWaveSpawnTimer = 3f;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer <= 0f)
                {
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer <= 0f)
                    {
                        nextEnemySpawnTimer = Random.Range(0f, 0.2f);
                        Enemy.Create(spawnPosition + UtilsClass.GetRandomDirection() * Random.Range(0f, 10f));
                        remainingEnemySpawnAmount--;

                        if (remainingEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingToNextWave;
                            spawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPosition.position = spawnPosition;
                            nextWaveSpawnTimer = 10f;
                        }
                    }

                }
                break;
            default:
                break;
        }

    }

    private void SpawnWave() 
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyWaveStarting);
        remainingEnemySpawnAmount = 5 + 3 * waveNumber;
        state = State.SpawningWave;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this,System.EventArgs.Empty);
    }

    public int GetWaveNumber() 
    {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer() 
    {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition() 
    {
        return spawnPosition;
    }
}

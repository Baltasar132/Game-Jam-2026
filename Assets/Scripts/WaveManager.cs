using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveManager INSTANCE;

    public static WaveManager Get() => INSTANCE;

    [Header("Info de las Waves")]
    [SerializeField] private List<WaveData> waves = new();
    [HideInInspector] public WaveData currentWave;
    [HideInInspector] public int currentWaveIdx = 0;
    [HideInInspector] public int maxWaveId = 0;
    [HideInInspector] public float currentCountDown = 0;

    public static int MaxWaveId => INSTANCE.maxWaveId;
    public static int CurrentWaveId => INSTANCE.currentWaveIdx;
    public static float Progress() => Mathf.Clamp01(INSTANCE.currentCountDown / INSTANCE.waves[CurrentWaveId].CountDown);

    void Awake()
    {
        INSTANCE = this;
        currentWave = waves[currentWaveIdx];
        currentCountDown = currentWave.CountDown;
        maxWaveId = waves.Count - 1;
    }

    void FixedUpdate()
    {
        currentCountDown -= Time.fixedDeltaTime;
        if (currentCountDown <= 0)
        {
            print("Spawning: " + currentWave.EnemyCount + " from " + currentWave.Direction);
            switch (currentWave.Direction)
            {
                case Direction.Top: Enemies.SpawnEnemyTop(currentWave.EnemyCount); break;
                case Direction.Right: Enemies.SpawnEnemyTop(currentWave.EnemyCount); break;
                case Direction.Down: Enemies.SpawnEnemyTop(currentWave.EnemyCount); break;
                case Direction.Left: Enemies.SpawnEnemyTop(currentWave.EnemyCount); break;
            }

            currentWaveIdx += 1;
            if (waves.Count > currentWaveIdx)
            {
                currentWave = waves[currentWaveIdx];
                currentCountDown = currentWave.CountDown;
            }
            else
            {
                // TODO: last wave
                currentCountDown = 1000000f;
            }
        }
    }

    [Serializable]
    public class WaveData
    {
        [SerializeField, Range(0f, 100f)] private float countDown = 10f;
        [SerializeField, Range(0, 100)] private int enemyCount = 10;
        [SerializeField] private Direction direction = Direction.Top;

        public float CountDown => countDown;
        public int EnemyCount => enemyCount;
        public Direction Direction => direction;
    }

    public enum Direction
    {
        Top, Right, Down, Left
    }
}

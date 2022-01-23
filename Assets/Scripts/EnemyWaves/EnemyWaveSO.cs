using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveName", menuName = "ScriptableObject/EnemyWave", order = 1)]
[System.Serializable]
public class EnemyWaveSO : ScriptableObject
{
    [Header("Enemy Prefabs")]
    public GameObject enemyPrefab;
    public GameObject powerUp;

    [Header("Timings")]
    public float waveInterval;
    public int waveCount;
}

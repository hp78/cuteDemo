////
// Description : Data for a type of enemy wave. 
//                  Determines what type of enemy spawns for the wave, 
//                  the powerup the last enemy drops and it's timing intervals.
////

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

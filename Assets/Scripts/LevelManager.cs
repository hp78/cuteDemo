////
// Description : Keeps track of the enemy waves, their timings and the end timing of the level.
//                  Each enemy wave instantiates its own monohaviour and updates independantly.
////

using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Struct for wave spawning. The type of enemy that spawns and its timing it spawns during the level.
    /// </summary>
    [System.Serializable]
    public struct EnemyWaveTiming
    {
        public EnemyWaveSO enemyWaveData;
        public float timing;
    }

    // List of waves timings
    [Header("Enemy Waves")]
    [SerializeField]
    public List<EnemyWaveTiming> waveTimings = new List<EnemyWaveTiming>();

    // EnemyWave helps with spawning enemy instances according to the wave data
    public GameObject enemyWavePrefab;

    // Timing the level should end
    public float endTiming;
    float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Create a sub-manager for each wave
        for(int i = 0; i < waveTimings.Count; ++i)
        {
            GameObject waveObject = Instantiate(enemyWavePrefab, transform);
            waveObject.GetComponent<EnemyWaveBehaviour>().InitializeData(waveTimings[i].enemyWaveData, waveTimings[i].timing);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Set level as cleared when exceed threshold
        elapsedTime += Time.deltaTime;
        if (elapsedTime > endTiming)
        {
            GameManager.instance.LevelCleared();
        }
    }
}

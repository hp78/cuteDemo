using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyWaveTiming
    {
        public EnemyWaveSO enemyWaveData;
        public float timing;
    }

    [Header("Enemy Waves")]
    [SerializeField]
    public List<EnemyWaveTiming> waveTimings = new List<EnemyWaveTiming>();
    public GameObject enemyWavePrefab;

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
    void Update()
    {
        
    }
}

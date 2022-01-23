using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveBehaviour : MonoBehaviour
{
    [Header("Wave Data")]
    public EnemyWaveSO waveData;
    float timeElapsed = 0;
    float startTime = 0;
    int enemyCounter = 0;

    // Initialize wave data from scriptable object
    public void InitializeData(EnemyWaveSO nWaveData, float nStartTime)
    {
        waveData = nWaveData;
        startTime = nStartTime;
    }

    // Update
    void Update()
    {
        // spawn enemy at the correct time
        timeElapsed += Time.deltaTime;

        if(timeElapsed > startTime && enemyCounter < waveData.waveCount)
        {
            if(timeElapsed > (startTime + waveData.waveInterval * enemyCounter))
            {
                GameObject enemyObject = Instantiate(waveData.enemyPrefab);
                ++enemyCounter;
                if(enemyCounter == waveData.waveCount && waveData.powerUp != null)
                {
                    enemyObject.GetComponent<EnemyBehaviour>().powerupPrefab = waveData.powerUp;
                }
            }
        }
    }
}

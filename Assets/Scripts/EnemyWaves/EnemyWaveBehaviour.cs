////
// Description : Behaviour for an instantiated enemy wave. 
//                  Functions as a helper object that spawns the enemy waves at their proper timings.
////

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
        // Spawn enemy at the correct time
        timeElapsed += Time.deltaTime;

        if(timeElapsed > startTime && enemyCounter < waveData.waveCount)
        {
            if(timeElapsed > (startTime + waveData.waveInterval * enemyCounter))
            {
                GameObject enemyObject = Instantiate(waveData.enemyPrefab);
                ++enemyCounter;

                // Give the last enemy spawned a power up drop
                if(enemyCounter == waveData.waveCount && waveData.powerUp != null)
                {
                    enemyObject.GetComponent<EnemyBehaviour>().powerupPrefab = waveData.powerUp;
                }
            }
        }
    }
}

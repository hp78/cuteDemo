using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnTiming
{
    public float time;
    public int enemyType;
    public GameObject dropPrefab;
}
public class LevelManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public List<GameObject> enemyPrefabs;

    [Header("Enemy Spawn Timings")]
    [SerializeField]
    public List<SpawnTiming> spawnTimings = new List<SpawnTiming>();

    float timeElapsed = 0f;
    int currIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // spawn an enemy if time passes time
        timeElapsed += Time.deltaTime;
        if(currIndex < spawnTimings.Count)
        {
            if(spawnTimings[currIndex].time < timeElapsed)
            {
                GameObject go = Instantiate(enemyPrefabs[spawnTimings[currIndex].enemyType]);
                go.GetComponent<EnemyBehaviour>().powerupPrefab = spawnTimings[currIndex].dropPrefab;
                ++currIndex;
            }
        }
    }
}

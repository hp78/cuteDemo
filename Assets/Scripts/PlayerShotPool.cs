using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotPool : MonoBehaviour
{
    public static PlayerShotPool instance;

    [Header("Shot prefabs")]
    public GameObject basicShotPrefab;
    public GameObject spreadShotPrefab;
    public GameObject laserShotPrefab;

    // Shots pool
    PlayerShot[] basicShotPool = new PlayerShot[30];
    PlayerShot[] spreadShotPool = new PlayerShot[20];
    PlayerShot[] laserShotPool = new PlayerShot[20];

    // Shot counters
    int basicShotCounter = 0;
    int spreadShotCounter = 0;
    int laserShotCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InitializePools();
    }

    // Instantiate enough shots for object pooling
    void InitializePools()
    {
        // Initialize pool for basic shots
        for(int i = 0; i < basicShotPool.Length; ++i)
        {
            basicShotPool[i] = Instantiate(basicShotPrefab,transform).GetComponent<PlayerShot>();
            basicShotPool[i].gameObject.SetActive(false);
        }

        // Initialize pool for spread shots
        for (int i = 0; i < spreadShotPool.Length; ++i)
        {
            spreadShotPool[i] = Instantiate(spreadShotPrefab, transform).GetComponent<PlayerShot>();
            spreadShotPool[i].gameObject.SetActive(false);
        }

        // Initialize pool for laser shots
        for (int i = 0; i < laserShotPool.Length; ++i)
        {
            laserShotPool[i] = Instantiate(laserShotPrefab, transform).GetComponent<PlayerShot>();
            laserShotPool[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Spawn a basic shot at given position
    /// </summary>
    /// <param name="spawnPos">Vector3 Position the shot spawns at</param>
    public void SpawnBasicShot(Vector3 spawnPos)
    {
        basicShotPool[basicShotCounter].gameObject.SetActive(true);
        basicShotPool[basicShotCounter].transform.position = spawnPos;
        ++basicShotCounter;

        // pool counter loops around
        if (basicShotCounter >= basicShotPool.Length)
            basicShotCounter = 0;
    }

    /// <summary>
    /// Spawns a spread shot at given position, direction vector also dictates rotation and direction
    /// </summary>
    /// <param name="spawnPos">Vector3 Position the shot spawns at</param>
    /// <param name="direction">Vector3 Direction(and infered rotation) of projectile</param>
    public void SpawnSpreadShot(Vector3 spawnPos, Vector3 direction)
    {
        spreadShotPool[spreadShotCounter].gameObject.SetActive(true);
        spreadShotPool[spreadShotCounter].SetDirection(direction);
        spreadShotPool[spreadShotCounter].transform.position = spawnPos;
        ++spreadShotCounter;

        // pool counter loops around
        if (spreadShotCounter >= spreadShotPool.Length)
            spreadShotCounter = 0;
    }

    /// <summary>
    /// Spawns a laser shot at given position
    /// </summary>
    /// <param name="spawnPos">Vector3 Position the shot spawns at</param>
    public void SpawnLaserShot(Vector3 spawnPos)
    {
        laserShotPool[laserShotCounter].gameObject.SetActive(true);
        laserShotPool[laserShotCounter].transform.position = spawnPos;
        ++laserShotCounter;

        // pool counter loops around
        if (laserShotCounter >= laserShotPool.Length)
            laserShotCounter = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotPool : MonoBehaviour
{
    public static PlayerShotPool instance;

    public GameObject basicShotPrefab;
    public GameObject spreadShotPrefab;
    public GameObject laserShotPrefab;

    PlayerShot[] basicShotPool = new PlayerShot[30];
    PlayerShot[] spreadShotPool = new PlayerShot[20];
    PlayerShot[] laserShotPool = new PlayerShot[50];

    int basicShotCounter = 0;
    int spreadShotCounter = 0;
    int laserShotCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InitializePools();
    }

    //
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

    public void SpawnBasicShot(Vector3 spawnPos)
    {
        basicShotPool[basicShotCounter].gameObject.SetActive(true);
        basicShotPool[basicShotCounter].transform.position = spawnPos;
        ++basicShotCounter;
        if (basicShotCounter >= basicShotPool.Length)
            basicShotCounter = 0;
    }

    public void SpawnSpreadShot(Vector3 spawnPos, Vector3 direction)
    {
        spreadShotPool[spreadShotCounter].gameObject.SetActive(true);
        spreadShotPool[spreadShotCounter].SetDirection(direction);
        spreadShotPool[spreadShotCounter].transform.position = spawnPos;
        ++spreadShotCounter;
        if (spreadShotCounter >= spreadShotPool.Length)
            spreadShotCounter = 0;
    }

    public void SpawnLaserShot(Vector3 spawnPos)
    {
        laserShotPool[laserShotCounter].gameObject.SetActive(true);
        laserShotPool[laserShotCounter].transform.position = spawnPos;
        ++laserShotCounter;
        if (laserShotCounter >= laserShotPool.Length)
            laserShotCounter = 0;
    }
}

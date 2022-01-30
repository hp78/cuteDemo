////
// Description : Handles enemy behaviour logic. Also stores enemy attributes and power ups it will drop.
////


using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Enemy health points
    public int enemyHealth = 10;
    SpriteRenderer spriteRend;

    // Enemy powerup drop
    public GameObject powerupPrefab;

    // Enemy bullet prefab
    public GameObject enemyShotPrefab;

    // Death explosion prefab
    public GameObject deathExplosionPrefab;

    // How many points this enemy is worth
    public int score = 100; // 100 default

    // Alive bool check
    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();

        // Set the enemy health according to difficulty when they spawn
        enemyHealth = (int)(enemyHealth * GameManager.instance.currPlayerStat.difficultyModifier);
    }

    /// <summary>
    /// Make the enemy receive damage
    /// </summary>
    /// <param name="damage">int amount of damage</param>
    public void ReceiveDamage(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth < 1 && isAlive)
        {
            // Set enemy as dead to prevent segment running mulitple times
            isAlive = false;

            // If enemy has powerup assigned, spawn one at death position
            if(powerupPrefab != null && gameObject.activeInHierarchy)
            {
                Instantiate(powerupPrefab, transform.position, Quaternion.identity);
            }
            StopAllCoroutines();

            GameManager.instance.AddScore(score);
            
            // Create fx for death explosion
            GameObject explosionObject = Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
            
            // Destroy explosion after 1sec and destroy self
            Destroy(explosionObject, 1f);
            Destroy(gameObject);

            return;
        }
        StartCoroutine(EnemyFlicker());
    }

    /// <summary>
    /// Coroutine to make enemy flicker
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyFlicker()
    {
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        spriteRend.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        spriteRend.color = Color.white;

        yield return null;
    }

    // For animation to call and disable enemy at the end of movement
    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    // For animation to call and enemy to do a shot
    public void StraightShot()
    {
        Instantiate(enemyShotPrefab, transform.position, Quaternion.identity);
    }

    // For animation to call and enemy to do a shot downwards
    public void StraightShotDown()
    {
        GameObject enemyShotObject = Instantiate(enemyShotPrefab, transform.position, Quaternion.identity);
        enemyShotObject.GetComponent<EnemyShot>().SetDirection(Vector3.down);
    }

    // For animation to call and enemy to do a shot upwards
    public void StraightShotUp()
    {
        GameObject enemyShotObject = Instantiate(enemyShotPrefab, transform.position, Quaternion.identity);
        enemyShotObject.GetComponent<EnemyShot>().SetDirection(Vector3.up);
    }
}

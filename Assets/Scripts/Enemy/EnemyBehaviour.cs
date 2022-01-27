using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // enemy health points
    public int enemyHealth = 10;
    SpriteRenderer spriteRend;

    // enemy powerup drop
    public GameObject powerupPrefab;

    // enemy bullet prefab
    public GameObject enemyShotPrefab;

    // death explosion prefab
    public GameObject deathExplosionPrefab;

    // how many points this enemy is worth
    public int score = 100; // 100 default

    // alive bool check
    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
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
            // set enemy as dead to prevent segment running mulitple times
            isAlive = false;

            // if enemy has powerup assigned, spawn one at death position
            if(powerupPrefab != null && gameObject.activeInHierarchy)
            {
                Instantiate(powerupPrefab, transform.position, Quaternion.identity);
            }
            StopAllCoroutines();

            GameManager.instance.AddScore(score);
            
            // create fx for death explosion
            GameObject explosionObject = Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
            // destroy explosion after 1sec and destroy self
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

    public void StraightShot()
    {
        Instantiate(enemyShotPrefab, transform.position, Quaternion.identity);
    }
}

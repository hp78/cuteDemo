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

        if (enemyHealth < 1)
        {
            // if enemy has powerup assigned, spawn one at death position
            if(powerupPrefab != null && gameObject.activeInHierarchy)
            {
                Instantiate(powerupPrefab, transform.position, Quaternion.identity);
            }
            StopAllCoroutines();
            gameObject.SetActive(false);

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
}

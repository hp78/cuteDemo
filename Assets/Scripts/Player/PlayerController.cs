////
// Description : Handles the player character. 
//                  Handles input from player and visual updates to the player object.
////

using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement speed of the player
    [Header("Player Movement")]
    public float playerSpeed = 10f;
    public float playerFocusSpeed = 5f;
    Vector3 moveVector;

    // References to player sprite elements
    [Header("Sprite References")]
    public SpriteRenderer hitboxSpriteRend;
    public SpriteRenderer playerSpriteRend;
    public Transform spritePivotTf;
    float pivotRotationOffset = 0f;

    // Player shot types
    public enum ShotType { BASIC, SPREAD, LASER };

    // Player state and elapsed state time
    public enum PlayerState { NORMAL, INVUL, DEATH, GAMEOVER };
    PlayerState currPlayerState = PlayerState.INVUL;
    float elapsedStateTime = 0f;

    // PlayerStat
    public PlayerStatSO playerStat;

    // Time from last shot
    float lastShot = 0.0f;

    // Reference to all the player shooting bits and their 2 sets of positions
    [Space(3)]
    public GameObject[] playerBits;
    [Space(3)]
    public Transform[] playerBitPos1;
    [Space(3)]
    public Transform[] playerBitPos2;

    // Variables to transition bit positions from normal to focus positions
    float bitTime = 0.0f;
    float bitMultiplier = -5.0f;

    // Audio source
    AudioSource audioSource;

    // Explosion reference
    public GameObject explosionFXPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize reference and variables
        audioSource = GetComponent<AudioSource>();
        moveVector = Vector3.zero;

        // Initialize visual elements
        RefreshBitState();
        GameManager.instance.UpdateStatText();
    }

    // Update is called once per frame
    void Update()
    {
        // Updates the state of player
        UpdatePlayerState();

        // Only respond when player is alive or invul
        if (currPlayerState == PlayerState.NORMAL || currPlayerState == PlayerState.INVUL)
        {
            UpdateMovement();
            UpdateSprite();
            UpdateShooting();
        }        
    }

    /// <summary>
    /// Updates all aspects related to the player character moving
    /// </summary>
    void UpdateMovement()
    {
        // set local variables
        moveVector = Vector3.zero;
        float speed = playerSpeed;

        /// Input chunk for player movement
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveVector += Vector3.up;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveVector += Vector3.down;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveVector += Vector3.left;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveVector += Vector3.right;
            }
        }

        // Check if player is focused mode
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = playerFocusSpeed;
        }

        // Move player transform according to input 
        transform.position += moveVector * speed * Time.deltaTime;

        // Tilts the player depending on vertical movement
        float newOffset = pivotRotationOffset + moveVector.y * (speed / playerSpeed) - moveVector.x * 0.1f * (speed / playerSpeed);
        pivotRotationOffset = Mathf.Clamp(newOffset, -30f, 30f);

        /// Bound player within screen
        {
            if (transform.position.x < -8f)
            {
                transform.position = new Vector3(-8f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 8f)
            {
                transform.position = new Vector3(8f, transform.position.y, transform.position.z);
            }

            if (transform.position.y < -4.5f)
            {
                transform.position = new Vector3(transform.position.x, - 4.5f, transform.position.z);
            }
            if (transform.position.y > 4.5f)
            {
                transform.position = new Vector3(transform.position.x, 4.5f, transform.position.z);
            }
        }
    }

    /// <summary>
    /// Update all elements of the player sprite
    /// </summary>
    void UpdateSprite()
    {
        // Lerp decay of sprite rotation
        pivotRotationOffset = Mathf.Lerp(pivotRotationOffset, 0f, Time.deltaTime * 5f);

        // Rotates the sprite about the pivot
        spritePivotTf.rotation = Quaternion.Euler(0, 0, pivotRotationOffset);

        // Shows the hitbox in focus mode
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            hitboxSpriteRend.enabled = true;
            bitMultiplier = 5.0f;
        }

        // Hides the hitbox leaving focus mode
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            hitboxSpriteRend.enabled = false;
            bitMultiplier = -5.0f;
        }

        // Update bit time
        float currBitTime = bitTime + Time.deltaTime * bitMultiplier;
        bitTime = Mathf.Clamp(currBitTime, 0, 1);

        // Update bit position
        for(int i = 0; i < playerBits.Length; ++i)
        {
            playerBits[i].transform.position = 
                Vector3.Lerp(playerBitPos1[i].position, playerBitPos2[i].position, bitTime);
        }
    }

    /// <summary>
    /// Updates all elements of the player shooting
    /// </summary>
    void UpdateShooting()
    {
        lastShot += Time.deltaTime;

        if (Input.GetKey(KeyCode.Z))
        {
            switch(playerStat.currShotType)
            {
                case ShotType.BASIC:
                    BasicShot();
                    break;

                case ShotType.SPREAD:
                    SpreadShot();
                    break;

                case ShotType.LASER:
                    LaserShot();
                    break;

                default:
                    BasicShot();
                    break;
            }
        }
    }

    /// <summary>
    /// Updates logic corresponding to current state of the player
    /// </summary>
    void UpdatePlayerState()
    {
        elapsedStateTime += Time.deltaTime;

        if (currPlayerState == PlayerState.INVUL)
        {
            if(elapsedStateTime > 2.0f)
                ChangePlayerState(PlayerState.NORMAL);
        }
        else if (currPlayerState == PlayerState.DEATH)
        {
            if (elapsedStateTime > 0.5f)
                ChangePlayerState(PlayerState.INVUL);
        }
    }

    /// <summary>
    /// Changes current player state to the given next state
    /// </summary>
    /// <param name="nextState"></param>
    void ChangePlayerState(PlayerState nextState)
    {
        if (nextState == currPlayerState)
        {
            return;
        }
        else if(nextState == PlayerState.NORMAL)
        {
            // reset timer and restore player to normal state and color
            elapsedStateTime = 0.0f;
            currPlayerState = PlayerState.NORMAL;
            playerSpriteRend.color = new Color(1, 1, 1, 1);
        }
        else if(nextState == PlayerState.INVUL)
        {
            // reset timer and set player invul and flicker
            elapsedStateTime = 0.0f;
            currPlayerState = PlayerState.INVUL;
            StartCoroutine(PlayerFlicker(2.0f));
        }
        else if(nextState == PlayerState.DEATH)
        {
            // reset timer and set player dead and not visible
            elapsedStateTime = 0.0f;
            currPlayerState = PlayerState.DEATH;
            playerSpriteRend.color = new Color(0, 0, 0, 0);
        }
    }

    /// <summary>
    /// Coroutine to make player flicker
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerFlicker(float flickerDuration)
    {
        float currRunningTime = 0.0f;
        while(currRunningTime < flickerDuration)
        {
            playerSpriteRend.color = Color.yellow;
            yield return new WaitForSeconds(0.1f);
            playerSpriteRend.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            currRunningTime += 0.2f;
        }
        playerSpriteRend.color = Color.white;
        yield return null;
    }

    /// <summary>
    /// Shoots basic pattern projectile
    /// </summary>
    void BasicShot()
    {
        // Shot cooldown
        if (lastShot < 0.15f) return;
        lastShot = 0;
        audioSource.Play();

        // Basic shot at 0 power
        PlayerShotPool.instance.SpawnBasicShot(transform.position + new Vector3(0, 0.1f));
        PlayerShotPool.instance.SpawnBasicShot(transform.position - new Vector3(0, 0.1f));

        // Basic shot at 1 power
        if (playerStat.currPower > 0)
        {
            PlayerShotPool.instance.SpawnBasicShot(playerBits[0].transform.position);
            PlayerShotPool.instance.SpawnBasicShot(playerBits[1].transform.position);
        }

        // Basic shot at 2
        if(playerStat.currPower > 1)
        {
            PlayerShotPool.instance.SpawnBasicShot(playerBits[2].transform.position);
            PlayerShotPool.instance.SpawnBasicShot(playerBits[3].transform.position);

        }

        // Spread shot at 3 or more power
        if (playerStat.currPower > 2)
        {
            PlayerShotPool.instance.SpawnBasicShot(playerBits[4].transform.position);
            PlayerShotPool.instance.SpawnBasicShot(playerBits[5].transform.position);
        }

        // Laser shot at 4 or more power
        if (playerStat.currPower > 3)
        {
            PlayerShotPool.instance.SpawnBasicShot(playerBits[4].transform.position, Vector3.left);
            PlayerShotPool.instance.SpawnBasicShot(playerBits[5].transform.position, Vector3.left);
        }
    }

    /// <summary>
    /// Shoots spread pattern projectile
    /// </summary>
    void SpreadShot()
    {
        // Shot cooldown
        if (lastShot < 0.15f) return;
        lastShot = 0;
        audioSource.Play();

        // float for the x in projectile direction vector
        float xVal = 1f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            xVal = 3f;
        }

        // Spread shot at 0 power
        PlayerShotPool.instance.SpawnBasicShot(transform.position + new Vector3(0, 0.3f));
        PlayerShotPool.instance.SpawnBasicShot(transform.position - new Vector3(0, 0.3f));

        // Spread shot at 1 power
        if (playerStat.currPower > 0)
        {
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[0].transform.position,
                new Vector3(xVal, 0.25f));
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[1].transform.position,
                new Vector3(xVal, -0.25f));
        }

        // Spread shot at 2 power
        if (playerStat.currPower > 1)
        {
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[2].transform.position,
                new Vector3(xVal, 0.4f));
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[3].transform.position,
                new Vector3(xVal, -0.4f));
        }

        // Spread shot at 3 power
        if (playerStat.currPower > 2)
        {
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[4].transform.position,
                new Vector3(xVal, 0.6f));
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[5].transform.position,
                new Vector3(xVal, -0.6f));
        }

        // Spread shot at 4 power or more
        if (playerStat.currPower > 3)
        {
            PlayerShotPool.instance.SpawnBasicShot(playerBits[4].transform.position);
            PlayerShotPool.instance.SpawnBasicShot(playerBits[5].transform.position);
        }
    }

    /// <summary>
    /// Shoots laser projectile
    /// </summary>
    void LaserShot()
    {
        // Shot cooldown
        if (lastShot < 0.075f) return;
        lastShot = 0;
        audioSource.Play();

        // Laser shot at 0 power
        PlayerShotPool.instance.SpawnLaserShot(transform.position + new Vector3(0, 0.2f));
        PlayerShotPool.instance.SpawnLaserShot(transform.position - new Vector3(0, 0.2f));

        // Laser shot at 1 power
        if (playerStat.currPower > 0)
        {
            PlayerShotPool.instance.SpawnLaserShot(playerBits[0].transform.position);
            PlayerShotPool.instance.SpawnLaserShot(playerBits[1].transform.position);
        }

        // Laser shot at 2 power
        if (playerStat.currPower > 1)
        {
            PlayerShotPool.instance.SpawnLaserShot(playerBits[2].transform.position);
            PlayerShotPool.instance.SpawnLaserShot(playerBits[3].transform.position);
        }

        // Laser shot at 3 power
        if (playerStat.currPower > 2)
        {
            PlayerShotPool.instance.SpawnLaserShot(playerBits[4].transform.position);
            PlayerShotPool.instance.SpawnLaserShot(playerBits[5].transform.position);
        }

        // Laser shot at 4 or more power
        if (playerStat.currPower > 3)
        {
            PlayerShotPool.instance.SpawnLaserShot(playerBits[4].transform.position, Vector3.up);
            PlayerShotPool.instance.SpawnLaserShot(playerBits[5].transform.position, -Vector3.up);
        }
    }

    /// <summary>
    /// Increase the power of the player shot
    /// </summary>
    public void PowerUp()
    {
        // Increment shot power
        ++playerStat.currPower;
        if (playerStat.currPower > 4)
            playerStat.currPower = 4;

        // Updates how many bits are active
        RefreshBitState();

        // Update UI
        GameManager.instance.UpdateStatText();
    }

    /// <summary>
    /// Shows/Hides the correct amount of bits floating in respect to player power level
    /// </summary>
    public void RefreshBitState()
    {
        playerBits[0].SetActive(playerStat.currPower > 0);
        playerBits[1].SetActive(playerStat.currPower > 0);
        playerBits[2].SetActive(playerStat.currPower > 1);
        playerBits[3].SetActive(playerStat.currPower > 1);
        playerBits[4].SetActive(playerStat.currPower > 2);
        playerBits[5].SetActive(playerStat.currPower > 2);
    }


    /// <summary>
    /// Change the current player shot type
    /// </summary>
    /// <param name="shotType"></param>
    public void ChangeShotType(ShotType shotType)
    {
        // Set according to new shot type pickup
        playerStat.currShotType = shotType;

        // free powerup if player is at 0 shot power
        if (playerStat.currPower == 0)
            PowerUp();

        // update ui
        GameManager.instance.UpdateStatText();
    }

    /// <summary>
    /// Kills the player
    /// </summary>
    public void PlayerDeath()
    {
        // Reduce player lives
        --playerStat.currLives;

        // if no more lives, game over
        if (playerStat.currLives < 0)
        {
            GameManager.instance.SetGameOver();
            return;
        }

        // Update ui
        GameManager.instance.UpdateStatText();
        
        // Set the player dead
        ChangePlayerState(PlayerState.DEATH);
        
        // Spawn explosion fx and destroy it afterwards
        GameObject explosionObject = Instantiate(explosionFXPrefab, transform.position, Quaternion.identity);
        Destroy(explosionObject, 1f);

        // Reduce player shot power to 1 if at higher power level
        if(playerStat.currPower > 1)
        {
            playerStat.currPower = 1;
            RefreshBitState();
        }
    }

    // Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If alive and hit enemy
        if(currPlayerState == PlayerState.NORMAL && 
            collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Kill player
            PlayerDeath();
        }

        // if alive and hit enemy bullet
        if (currPlayerState == PlayerState.NORMAL && 
            collision.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            // Kill player and disable enemy bullet
            PlayerDeath();
            collision.gameObject.SetActive(false);
        }
    }
}

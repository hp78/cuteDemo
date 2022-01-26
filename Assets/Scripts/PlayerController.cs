using System.Collections;
using System.Collections.Generic;
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

    // Player shots
    public enum ShotType { BASIC, SPREAD, LASER };
    [Header("Player Shots")] 
    public ShotType currShotType = ShotType.BASIC;
    public int shotPower = 0;
    float lastShot = 0.0f;

    [Space(3)]
    public GameObject[] playerBits;
    [Space(3)]
    public Transform[] playerBitPos1;
    [Space(3)]
    public Transform[] playerBitPos2;

    // variables to transition bit positions from normal to focus positions
    float bitTime = 0.0f;
    float bitMultiplier = -5.0f;


    // Start is called before the first frame update
    void Start()
    {
        moveVector = Vector3.zero;
        RefreshBitState();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateSprite();
        UpdateShooting();
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
            switch(currShotType)
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
    /// Shoots basic pattern projectile
    /// </summary>
    void BasicShot()
    {
        // Shot cooldown
        if (lastShot < 0.15f) return;
        lastShot = 0;

        // Basic shot at 0 power
        PlayerShotPool.instance.SpawnBasicShot(transform.position + new Vector3(0, 0.1f));
        PlayerShotPool.instance.SpawnBasicShot(transform.position - new Vector3(0, 0.1f));

        // Basic shot at 1 power
        if (shotPower > 0)
        {
            PlayerShotPool.instance.SpawnBasicShot(playerBits[0].transform.position);
            PlayerShotPool.instance.SpawnBasicShot(playerBits[1].transform.position);
        }

        // Basic shot at 2
        if(shotPower > 1)
        {
            PlayerShotPool.instance.SpawnBasicShot(playerBits[2].transform.position);
            PlayerShotPool.instance.SpawnBasicShot(playerBits[3].transform.position);

        }

        // Spread shot at 3 or more power
        if (shotPower > 2)
        {
            PlayerShotPool.instance.SpawnBasicShot(playerBits[4].transform.position);
            PlayerShotPool.instance.SpawnBasicShot(playerBits[5].transform.position);
        }

        // Laser shot at 4 or more power
        if (shotPower > 3)
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
        if (shotPower > 0)
        {
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[0].transform.position,
                new Vector3(xVal, 0.25f));
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[1].transform.position,
                new Vector3(xVal, -0.25f));
        }

        // Spread shot at 2 power
        if (shotPower > 1)
        {
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[2].transform.position,
                new Vector3(xVal, 0.4f));
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[3].transform.position,
                new Vector3(xVal, -0.4f));
        }

        // Spread shot at 3 power
        if (shotPower > 2)
        {
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[4].transform.position,
                new Vector3(xVal, 0.6f));
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[5].transform.position,
                new Vector3(xVal, -0.6f));
        }

        // Spread shot at 4 power or more
        if (shotPower > 3)
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

        // Laser shot at 0 power
        PlayerShotPool.instance.SpawnLaserShot(transform.position + new Vector3(0, 0.2f));
        PlayerShotPool.instance.SpawnLaserShot(transform.position - new Vector3(0, 0.2f));

        // Laser shot at 1 power
        if (shotPower > 0)
        {
            PlayerShotPool.instance.SpawnLaserShot(playerBits[0].transform.position);
            PlayerShotPool.instance.SpawnLaserShot(playerBits[1].transform.position);
        }

        // Laser shot at 2 power
        if (shotPower > 1)
        {
            PlayerShotPool.instance.SpawnLaserShot(playerBits[2].transform.position);
            PlayerShotPool.instance.SpawnLaserShot(playerBits[3].transform.position);
        }

        // Laser shot at 3 power
        if (shotPower > 2)
        {
            PlayerShotPool.instance.SpawnLaserShot(playerBits[4].transform.position);
            PlayerShotPool.instance.SpawnLaserShot(playerBits[5].transform.position);
        }

        // Laser shot at 4 or more power
        if (shotPower > 3)
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
        ++shotPower;

        RefreshBitState();
        GameManager.instance.UpdateStatText(3, shotPower, currShotType);
    }

    /// <summary>
    /// Shows/Hides the correct amount of bits floating in respect to player power level
    /// </summary>
    public void RefreshBitState()
    {
        playerBits[0].SetActive(shotPower > 0);
        playerBits[1].SetActive(shotPower > 0);
        playerBits[2].SetActive(shotPower > 1);
        playerBits[3].SetActive(shotPower > 1);
        playerBits[4].SetActive(shotPower > 2);
        playerBits[5].SetActive(shotPower > 2);
    }


    /// <summary>
    /// Change the current player shot type
    /// </summary>
    /// <param name="shotType"></param>
    public void ChangeShotType(int shotType)
    {
        switch(shotType)
        {
            case 0:
                currShotType = ShotType.BASIC;
                break;

            case 1:
                currShotType = ShotType.SPREAD;
                break;

            case 2:
                currShotType = ShotType.LASER;
                break;

            default:
                currShotType = ShotType.BASIC;
                break;
        }
        if (shotPower == 0)
            PowerUp();

        GameManager.instance.UpdateStatText(3, shotPower, currShotType);
    }

    /// <summary>
    /// Kills the player
    /// </summary>
    public void PlayerDeath()
    {

    }

    // Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            PlayerDeath();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyShot"))
        {
            PlayerDeath();
            collision.gameObject.SetActive(false);
        }
    }
}

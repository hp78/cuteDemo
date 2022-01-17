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
    [Space(3)]
    public GameObject[] playerBits;

    public int shotPower = 0;
    float lastShot = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        moveVector = Vector3.zero;
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
        }

        // Hides the hitbox leaving focus mode
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            hitboxSpriteRend.enabled = false;
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

    //
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

        // Basic shot at 2 or more power
        if(shotPower > 1)
        {
            PlayerShotPool.instance.SpawnBasicShot(playerBits[2].transform.position);
            PlayerShotPool.instance.SpawnBasicShot(playerBits[3].transform.position);

        }
    }

    //
    void SpreadShot()
    {
        // Shot cooldown
        if (lastShot < 0.15f) return;
        lastShot = 0;

        // Basic shot at 0 power
        PlayerShotPool.instance.SpawnBasicShot(transform.position + new Vector3(0, 0.3f));
        PlayerShotPool.instance.SpawnBasicShot(transform.position - new Vector3(0, 0.3f));

        // Basic shot at 1 power
        if (shotPower > 0)
        {
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[0].transform.position, 
                new Vector3(1,0.25f));
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[1].transform.position, 
                new Vector3(1,-0.25f));
        }

        // Basic shot at 2 or more power
        if (shotPower > 1)
        {
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[2].transform.position,
                new Vector3(1, 0.4f));
            PlayerShotPool.instance.SpawnSpreadShot(playerBits[3].transform.position,
                new Vector3(1, -0.4f));
        }
    }

    //
    void LaserShot()
    {
        // Shot cooldown
        if (lastShot < 0.075f) return;
        lastShot = 0;

        // Basic shot at 0 power
        PlayerShotPool.instance.SpawnLaserShot(transform.position + new Vector3(0, 0.2f));
        PlayerShotPool.instance.SpawnLaserShot(transform.position - new Vector3(0, 0.2f));

        // Basic shot at 1 power
        if (shotPower > 0)
        {
            PlayerShotPool.instance.SpawnLaserShot(playerBits[0].transform.position);
            PlayerShotPool.instance.SpawnLaserShot(playerBits[1].transform.position);
        }

        // Basic shot at 2 or more power
        if (shotPower > 1)
        {
            PlayerShotPool.instance.SpawnLaserShot(playerBits[2].transform.position);
            PlayerShotPool.instance.SpawnLaserShot(playerBits[3].transform.position);
        }
    }
}

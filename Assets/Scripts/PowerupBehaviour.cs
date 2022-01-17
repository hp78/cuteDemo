using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBehaviour : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * 2.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // power up the player and disable self
        collision.gameObject.GetComponent<PlayerController>().PowerUp();
        gameObject.SetActive(false);
    }
}

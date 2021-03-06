////
// Description : Behaviour for shot-type powerups in the game
////

using UnityEngine;

public class ShotTypePowerupBehaviour : MonoBehaviour
{
    public PlayerController.ShotType shotType = PlayerController.ShotType.BASIC;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * 2.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // change the shot type for the player and disable self
        collision.gameObject.GetComponent<PlayerController>().ChangeShotType(shotType);
        gameObject.SetActive(false);
    }
}

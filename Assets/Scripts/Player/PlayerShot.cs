////
// Description : Behaviour for the player bullet
////

using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    Vector3 directionVector = Vector3.right;
    public float bulletSpeed = 25;
    public int bulletDamage = 1;

    // Sets the direction and rotation of the shot
    public void SetDirection(Vector3 newDirection)
    {
        directionVector = newDirection.normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, directionVector);
    }

    // Update is called once per frame
    void Update()
    {
        // Update bullet position
        transform.position += directionVector * bulletSpeed * Time.deltaTime;

        // disable when out of bounds
        if ((transform.position.x < -10f) || 
            (transform.position.x > 10f) || 
            (transform.position.y < -6f) || 
            (transform.position.y > 6f))
        {
            gameObject.SetActive(false);
        }
    }

    // Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // collision layers only with enemy units
        gameObject.SetActive(false);
        EnemyBehaviour enemy = collision.gameObject.GetComponent<EnemyBehaviour>();
        enemy.ReceiveDamage(bulletDamage);
    }
}

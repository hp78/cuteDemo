////
// Description : Behaviour for the enemy bullet
////


using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    // Direction vector of the bullet
    Vector3 directionVector = Vector3.left;

    // Bullet speed
    public float bulletSpeed = 5;

    // Bullet Damage
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
}

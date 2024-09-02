using UnityEngine;

public class BulletController : MonoBehaviour
{
    /// <summary>
    /// Since bullet is a prefab, it starts flying to the right the moment it spawns.
    /// </summary>
    void Start()
    {
        this.GetComponent<Rigidbody2D>().velocity = transform.right * 10;
        Destroy(gameObject, 2); //destroys the bullet after 2 seconds (prevents clutter)
    }

    /// <summary>
    /// Bullet is a trigger to prevent weird physics after collision
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            //bullet only destroys itself. Damage dealt is handled in the EnemyController script.
            Destroy(gameObject); 
        }
    }
}

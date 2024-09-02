using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Variables
    [SerializeField] private int Health;
    [SerializeField] private int Speed;
    [SerializeField] private AudioSource Damaged;
    [SerializeField] private AudioSource Dies;
    [SerializeField] private GameObject TankExplosion;
    private Vector3 pointHit;
    private Animator anim;
    private GameManager gameManager;

    #endregion

    /// <summary>
    /// Since enemies are a prefab, they start moving to the left immediately after spawning.
    /// </summary>
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        this.GetComponent<Rigidbody2D>().velocity = transform.right * -Speed;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// If enemies touch a bullet, they lose 1 hp and start a "damaged" animation
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            pointHit = new Vector3(collision.transform.position.x, collision.transform.position.y, 
                collision.transform.position.z);

            //an explosion animation appears where they were hit
            Instantiate(TankExplosion, pointHit, Quaternion.identity);
            Health--;
            StartCoroutine(TakeDamage());
        }
    }

    /// <summary>
    /// Tells the GameManager to damage the player if they touch the player or go past them (behind the players tank is
    /// a huge invisible game object with a box collider that also has the Player tag)
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            gameManager.UpdateLives();
            Destroy(gameObject); //enemy instantly disappears after touching the player
        }
    }

    /// <summary>
    /// Animations for taking damage occur here. Includes death animation too. The enemy will temporarily stop moving
    /// after being hit, and GameManager will update score if enemies die.
    /// </summary>
    /// <returns></returns>
    IEnumerator TakeDamage()
    {
        anim.SetTrigger("Hurt");
        Damaged.Play(); //sound effect that plays when bullet collides with the enemy
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (Health == 0)
        {
            anim.SetBool("Dead", true);
            Dies.Play(); //sound effect that plays when the enemy reaches 0 health
            yield return new WaitForSeconds(0.6f);
            gameManager.UpdateScore();
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(0.4f);
        this.GetComponent<Rigidbody2D>().velocity = transform.right * -Speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int Health;
    [SerializeField] private int Speed;
    [SerializeField] private AudioSource Damaged;
    [SerializeField] private AudioSource Dies;
    [SerializeField] private GameObject TankExplosion;
    private Vector3 pointHit;
    private Animator anim;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        this.GetComponent<Rigidbody2D>().velocity = transform.right * -Speed;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            pointHit = new Vector3(collision.transform.position.x, collision.transform.position.y, 
                collision.transform.position.z);
            Instantiate(TankExplosion, pointHit, Quaternion.identity);
            Health--;
            StartCoroutine(TakeDamage());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            gameManager.UpdateLives();
            Destroy(gameObject);
        }
    }

    IEnumerator TakeDamage()
    {
        anim.SetTrigger("Hurt");
        Damaged.Play();
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (Health == 0)
        {
            anim.SetBool("Dead", true);
            Dies.Play();
            yield return new WaitForSeconds(0.57f);
            gameManager.UpdateScore();
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(0.4f);
        this.GetComponent<Rigidbody2D>().velocity = transform.right * -Speed;
    }
}

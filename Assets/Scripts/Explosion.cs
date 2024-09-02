using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    /// <summary>
    /// When the player shoots, the explosion prefab first appears over the bullet. The only command it gets is to
    /// destroy itself after 0.2 seconds (after the full animation plays)
    /// </summary>
    void Start()
    {
        StartCoroutine(DestroyExplosion());
    }

    /// <summary>
    /// A coroutine that destroys the explosion animation after 0.2 seconds (lets the explosion animation play before
    /// making it disappear).
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}

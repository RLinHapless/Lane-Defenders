using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject Bullet;
    [SerializeField] private GameObject TankTip;
    [SerializeField] private PlayerInput pInputs;
    [SerializeField] private GameObject TankExplosion;
    [SerializeField] private AudioSource Fire;
    private InputAction move;
    private InputAction shoot;
    private InputAction restart;
    private InputAction quit;

    private float moveDirection;
    private float speed = 10;
    private bool isMoving;
    private bool isShooting;
    private bool canShoot;
    #endregion

    /// <summary>
    /// Activates and assigns all player inputs.
    /// </summary>
    void Start()
    {
        pInputs.currentActionMap.Enable();
        move = pInputs.currentActionMap.FindAction("Move");
        shoot = pInputs.currentActionMap.FindAction("Shoot");
        restart = pInputs.currentActionMap.FindAction("restart");
        quit = pInputs.currentActionMap.FindAction("quit");

        move.started += MoveStarted;
        move.canceled += MoveCanceled;
        shoot.started += ShootStarted;
        shoot.canceled += ShootCanceled;
        restart.started += RestartStarted;
        quit.started += QuitStarted;

        canShoot = true;
    }

    /// <summary>
    /// a function that's called from the GameManager which prevents the player from moving and shooting. Whisks the
    /// tank extremely far away to give the illusion of being destroyed (I don't like destroying the player because
    /// it can cause issues).
    /// </summary>
    public void Lose()
    {
        move.started -= MoveStarted;
        move.canceled -= MoveCanceled;
        shoot.started -= ShootStarted;
        shoot.canceled -= ShootCanceled;
        this.transform.position = new Vector2(999, 999);
    }

    /// <summary>
    /// Removes all player inputs if the players tank is ever destroyed (includes reloading the scene)
    /// </summary>
    private void OnDestroy()
    {
        move.started -= MoveStarted;
        move.canceled -= MoveCanceled;
        shoot.started -= ShootStarted;
        shoot.canceled -= ShootCanceled;
        restart.started -= RestartStarted;
        quit.started -= QuitStarted;
    }

    private void QuitStarted(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }

    private void RestartStarted(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(0);
    }

    private void ShootCanceled(InputAction.CallbackContext obj)
    {
        isShooting = false;
    }

    private void ShootStarted(InputAction.CallbackContext obj)
    {
        isShooting = true;
    }
    private void MoveCanceled(InputAction.CallbackContext obj)
    {
        isMoving = false;
    }

    private void MoveStarted(InputAction.CallbackContext obj)
    {
        isMoving = true;
    }

    /// <summary>
    /// Controls the players movements, and their shooting
    /// </summary>
    private void FixedUpdate()
    {
        if(isMoving)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed * moveDirection);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if(isShooting && canShoot)
        {
            StartCoroutine(BulletDownTime());
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            //moves the player in the direction they're pressing by reading the positive or negative value
            moveDirection = move.ReadValue<float>(); 
        }
    }

    /// <summary>
    /// Controls the rate at which bullets can be fired. Allows for the player to hold fire.
    /// </summary>
    /// <returns></returns>
    IEnumerator BulletDownTime()
    {
        //first makes the explosion animation appear
        Instantiate(TankExplosion, new Vector2(TankTip.transform.position.x, TankTip.transform.position.y),
            this.transform.rotation);
        canShoot = false;
        Fire.Play(); //a sound effect will play whenever the player shoots
        Instantiate(Bullet, new Vector2(TankTip.transform.position.x, TankTip.transform.position.y),
            this.transform.rotation);
        yield return new WaitForSeconds(0.62f);
        canShoot = true;
    }
}

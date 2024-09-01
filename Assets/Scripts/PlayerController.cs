using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
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

    // Start is called before the first frame update
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

    public void Lose()
    {
        move.started -= MoveStarted;
        move.canceled -= MoveCanceled;
        shoot.started -= ShootStarted;
        shoot.canceled -= ShootCanceled;
        this.transform.position = new Vector2(999, 999);
    }

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
            moveDirection = move.ReadValue<float>();
        }
    }

    IEnumerator BulletDownTime()
    {
        Instantiate(TankExplosion, new Vector2(TankTip.transform.position.x, TankTip.transform.position.y),
            this.transform.rotation);
        canShoot = false;
        Fire.Play();
        Instantiate(Bullet, new Vector2(TankTip.transform.position.x, TankTip.transform.position.y),
            this.transform.rotation);
        yield return new WaitForSeconds(0.6f);
        canShoot = true;
    }
}

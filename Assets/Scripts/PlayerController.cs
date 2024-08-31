using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject Bullet;
    [SerializeField] private GameObject TankTip;
    [SerializeField] private PlayerInput pInputs;
    [SerializeField] private GameObject TankExplosion;
    [SerializeField] private AudioSource Fire;
    private InputAction move;
    private InputAction shoot;
    private InputAction pause;

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
        pause = pInputs.currentActionMap.FindAction("Pause");

        move.started += MoveStarted;
        move.canceled += MoveCanceled;
        shoot.started += ShootStarted;
        shoot.canceled += ShootCanceled;
        pause.started += PauseStarted;

        canShoot = true;
    }

    private void ShootCanceled(InputAction.CallbackContext obj)
    {
        isShooting = false;
    }

    private void ShootStarted(InputAction.CallbackContext obj)
    {
        isShooting = true;
    }

    private void PauseStarted(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
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
        TankExplosion.SetActive(true);
        canShoot = false;
        Fire.Play();
        Instantiate(Bullet, new Vector2(TankTip.transform.position.x, TankTip.transform.position.y),
            this.transform.rotation);
        yield return new WaitForSeconds(0.2f);
        TankExplosion.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        canShoot = true;
    }
}

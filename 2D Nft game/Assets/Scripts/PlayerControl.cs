using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    Controls control;

    public float moveSpeed = 5f; // Speed at which the player moves
    public float jumpForce = 10f; // Force applied when the player jumps

    public bool canMove;

    private Rigidbody2D rb;
    [SerializeField]private bool isGrounded;
    public Transform groundCheck; // Transform representing the ground check position
    public float groundCheckRadius = 0.2f; // Radius of the ground check circle
    public LayerMask groundLayer;// Layer mask for the ground

    bool isRightHit;
    Vector2 boxSize = new Vector2(.1f, 1f);
    public Transform rightCheck;




    Animator animator;

    LevelController levelController;

    short lifeValue;

    [SerializeField]Transform launchPos;
    [SerializeField] Vector2 throwForce;
    bool isPowerOn;
    short hitValue = 3;

    [SerializeField] GameObject[] activeSkin;


    [SerializeField]short jumpValue;

    bool canControl;

    float joystickMovementValue;
  

    bool isCanJump;

    [SerializeField] float maxY, minY;

    [SerializeField] float fuelPowerUpTime;
    bool isFuelPowerUp;


    bool isCatchBee;
    [SerializeField]float catchRadious = 2f;
    [SerializeField]Transform catchPostion;
    public LayerMask EnemyLayer;

    private void Awake()
    {
        control = new Controls();
        levelController = FindObjectOfType<LevelController>();
    }

    private void OnEnable()
    {
        control.Enable();
        control.Player.Joystick.performed += ctx => JoystickMove(ctx);
        control.Player.Joystick.canceled += ctx => JoystickMoveCancel();
        control.Player.Shoot.performed += ctx => Shoot();
        control.Player.ThrowFruit.performed += ctx => ThrowFruit();  
        control.Player.Jump.started += ctx => JumpProcess();  

    }

    private void JoystickMoveCancel()
    {
      joystickMovementValue = 0;
    }

    private void JoystickMove(InputAction.CallbackContext context)
    {
        joystickMovementValue = context.ReadValue<float>();
     
    }

    private void JumpProcess()
    {
        isCanJump = true;
    }

    private void OnDisable()
    {
        control.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lifeValue = 3;
        levelController.UpdateLifeValue(lifeValue);

        canControl = true;

        switch (levelController.playerMode) 
        {
                case LevelController.PlayerMode.OnFoot:
                rb.gravityScale = 1;
                SkinActive(0);               
                animator = activeSkin[0].GetComponent<Animator>();
                ; break;

                case LevelController.PlayerMode.OnPlane: SkinActive(1);
                  rb.gravityScale = 0;
                animator = activeSkin[1].GetComponent<Animator>();
                ; break;
                case LevelController.PlayerMode.OnRocket: 
                SkinActive(2);
                rb.gravityScale = 0;
                animator = activeSkin[2].GetComponent<Animator>();
                ; break;
        }

       
    }

    private void Update()
    {
        // Check if the player is grounded
        if (canControl)
        {

            switch (levelController.playerMode)
            {
                case LevelController.PlayerMode.OnFoot:
                    isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

                    isRightHit = Physics2D.OverlapBox(rightCheck.position, boxSize, 0f, groundLayer);

                    if (isRightHit) 
                    {
                        Debug.Log("RightHIt");
                    }

                    if (animator != null)
                    {
                        animator.SetFloat("Speed", canMove ? moveSpeed : 0);
                        animator.SetBool("isJumping", !isGrounded);
                    }

                    if (isCanJump)
                    {
                        isCanJump = false;

                        if (canMove && isGrounded)
                        {
                            jumpValue = 0;
                            Jump();
                            jumpValue++;
                        }
                        else
                        {
                            if (!isGrounded && jumpValue == 1)
                            {
                                Jump();
                                jumpValue = 0;
                            }
                        }

                        Debug.Log(jumpValue);
                    }


                    ; break;

                case LevelController.PlayerMode.OnPlane:

                    transform.Translate(new Vector2(0,joystickMovementValue)*moveSpeed * Time.deltaTime);

                    if (transform.position.y >= maxY) { transform.position = new Vector2(transform.position.x, maxY); }
                    else if (transform.position.y <= minY) { transform.position = new Vector2(transform.position.x, minY); }

                    break;


                case LevelController.PlayerMode.OnRocket:

                    transform.Translate(new Vector2(0, joystickMovementValue) * moveSpeed * Time.deltaTime);

                    if (transform.position.y >= maxY) { transform.position = new Vector2(transform.position.x, maxY); }
                    else if (transform.position.y <= minY) { transform.position = new Vector2(transform.position.x, minY); }

                    break;


            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

   


    private void Move()
    {
        // Move the player to the right continuously
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
    // Apply a vertical force to the player to make them jump
        if (levelController.playerMode == LevelController.PlayerMode.OnFoot)
        {

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            GameManager.Instance.PlaySound("Jump");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);
            levelController.UpdateCoinValue(1);
            GameManager.Instance.PlaySound("CoinCollect");
        }
        else if (collision.CompareTag("Bee"))
        {
            HitAndDead();

            Pooler.Instance.GetVfx("hitVfx", new Vector2(transform.position.x + .5f, transform.position.y + 1));
        }

        else if (collision.CompareTag("Meat"))
        {
            collision.gameObject.SetActive(false);
            if (!isPowerOn)
            {
                isPowerOn = true;
                ActiveMeetPowerUp();
            }
        }

        else if (collision.CompareTag("Fruits"))
        {
            levelController.IncreaseFruitsValue();
            collision.gameObject.SetActive(false);
            GameManager.Instance.PlaySound("EatFruit");
        }

        else if (collision.CompareTag("LevelEnd"))
        {
            levelController.LevelComplete();  
            canControl = false;
            canMove = false;
            moveSpeed = 0;
            rb.velocity = Vector2.zero;
        }
        else if (collision.CompareTag("FuelBarrel"))
        {
            collision.gameObject.SetActive(false);
            moveSpeed = moveSpeed * 2;
            GameManager.Instance.PlaySound("FuelCollect");
            StartCoroutine(FuelBarrelDeactivePowerUp());
            
            
        }
    }

   


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Statue")) 
        {
            if (isRightHit) 
            {
       
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
                HitAndDead();
            }

          
        }
    }

    public void HitAndDead()
    {
        if (isPowerOn)
        {
          

            hitValue--;

            if (hitValue < 1) 
            {
                hitValue = 3;
                lifeValue--;
            }
        }
        else
        {
            lifeValue--;
        }

        if (animator != null)
        {
            animator.SetTrigger("hurt");
           
        }

        GameManager.Instance.PlaySound("BeeHit");

        if (lifeValue < 1)
        {
            Debug.Log("Dead");
            lifeValue = 0;
            moveSpeed = 0;
            canMove = false;
            levelController.GameOver();



            gameObject.SetActive(false);

        }
        else
        {
            canMove = false;
            StartCoroutine(StartMoveAgain());
        }

        levelController.UpdateLifeValue(lifeValue);
    }

    IEnumerator StartMoveAgain()
    {
        yield return new WaitForSeconds(1.5f);
        canMove = true;
    }

    private void Shoot()
    {
        // Implement shooting functionality here


        switch (levelController.playerMode)
        {
            case LevelController.PlayerMode.OnFoot:
                animator.SetTrigger("throw");
                ; break;

            case LevelController.PlayerMode.OnPlane:
                GameObject bullet = Pooler.Instance.GetPooledObject("Bullet");
                 bullet.transform.position = launchPos.position ;
                bullet.SetActive(true);

                ; break;

        }

        GameManager.Instance.PlaySound("Shoot");

      
    }

    public void ThrwoConfigure() 
    {
        GameManager.Instance.PlaySound("Throw");
     

        GameObject throwObject = Pooler.Instance.GetPooledObject("throwA");
       // GameObject throwObject = Pooler.Instance.GetPooledObject("Fruit");
        if (throwObject != null) 
        {
            throwObject.transform.position = launchPos.position;
            throwObject.SetActive(true);
            throwObject.GetComponent<Rigidbody2D>().velocity = throwForce;

        }
    }


    private void ThrowFruit()
    {
       
        GameObject throwObject = Pooler.Instance.GetPooledObject("FruitEat");
        if (throwObject != null)
        {
            throwObject.transform.position = launchPos.position;
            throwObject.SetActive(true);
            throwObject.GetComponent<Rigidbody2D>().velocity = throwForce;

        }
    }

    private void ActiveMeetPowerUp() 
    {
        transform.GetChild(0).localScale = new Vector3(.23f, .23f, .23f);
        StartCoroutine(DeactiveMeetPowerUp());

        isFuelPowerUp = true;
    }

    IEnumerator DeactiveMeetPowerUp() 
    {
        moveSpeed *= 2;
        yield return new WaitForSeconds(5);
        transform.GetChild(0).localScale = new Vector3(.18f, .18f, .18f);
        moveSpeed /= 2;
        isPowerOn = false;
        hitValue = 3;

        isFuelPowerUp = false;
    }

    IEnumerator FuelBarrelDeactivePowerUp() 
    {
        yield return new WaitForSeconds(fuelPowerUpTime);
        moveSpeed = 5;
    }


    public void SkinActive(short index) 
    {

        foreach (GameObject skin in activeSkin) 
        {
            skin.SetActive(false);
        }

        activeSkin[index].SetActive(true);
    }


    public void IsHitCheckBee() 
    {
        Collider2D  hit = Physics2D.OverlapCircle(catchPostion.position,catchRadious,EnemyLayer);

        if (hit != null) 
        {
            if (hit.CompareTag("Bee")) 
            {
                hit.GetComponent<EnemyBee>().Dead();
            }
        }

    }


}

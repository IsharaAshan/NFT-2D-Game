using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    //Classes
    Controls inputControls;
    GameManager gameManager;
    [SerializeField]LevelManager levelManager;
    [SerializeField] PlayerKeepBounds keepBoundsSetUp; 

    //Components
    [SerializeField]Animator animator;
    [SerializeField]Rigidbody2D rb;
 

    //Skins
    [SerializeField] GameObject[] playerSkins;


    // Jump settings
    [SerializeField] float jumpForce;
    bool isDoubleJump;
    bool isGrounded;

    // Ground check settings
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    //
    [SerializeField]Transform throwHand;
    [SerializeField] Vector2 thorowForce;

    byte playerModeValue = 0;
  
    public bool isCanControl;
    bool isHitStatue;
    float defaultGravityScale;
    bool isCanDamege;
    bool isFuelPoweUp;

    //Level 2
    [SerializeField]float bullertForce = 1;
    [SerializeField]Transform shootPos;
    private Vector2 moveInput;
    [SerializeField]float flySpeed = 8;

    //Level 3
    [SerializeField] Transform catchPoint;
    [SerializeField]LayerMask  enemyLayer;
    [SerializeField]Vector2 catchBoxSize;

    private void Awake()
    {
        inputControls = new Controls();
    }

    private void OnEnable()
    {
         inputControls.Enable();

        inputControls.Player.Jump.performed += OnJump; // Corrected event subscription for new Input System
        inputControls.Player.Throw.performed += OnThrow; // Corrected event subscription for new Input System
        inputControls.Player.Fruit.performed += OnFruitThrow; // Corrected event subscription for new Input System
        inputControls.Player.Move.performed += ctx => OnJoystickMove(ctx);
        inputControls.Player.Move.canceled += ctx => OnJoystickMove(ctx);
        //inputControls.Player.Catch.performed += OnCatchThrow; // Corrected event subscription for new Input System

        levelManager.OnGameStop.AddListener(OnStopTriger);
    }

   

    private void Start()
    {
        gameManager = GameManager.Instance;

        foreach (GameObject p in playerSkins) 
        {
            p.SetActive(false);
        }

        switch (levelManager.playerMode) 
        {
            case LevelManager.PlayerMode.Ground:

                playerSkins[0].SetActive(true);
                animator = playerSkins[0].GetComponent<Animator>();
                rb.gravityScale = 3;
                animator.SetInteger("PlayerState", 1);
                playerModeValue = 0;

                break;

            case LevelManager.PlayerMode.Sky:

                playerSkins[1].SetActive(true);
                animator = playerSkins[1].GetComponent<Animator>();
                rb.gravityScale = 0;
                playerModeValue = 1;
                keepBoundsSetUp.SetUpAvatar(1);

                break;

            case LevelManager.PlayerMode.Space:

                playerSkins[2].SetActive(true);
                animator = playerSkins[2].GetComponent<Animator>();
               
                rb.gravityScale = 0;
                playerModeValue = 2;
                keepBoundsSetUp.SetUpAvatar(2);

                break;
        }

       
        defaultGravityScale = rb.gravityScale;
        isCanControl = true;
        isCanDamege = true;


    }

    private void Update()
    {
        if (isCanControl)
        {
            if (playerModeValue == 0)
            {
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

                if (isGrounded)
                {
                    isDoubleJump = false; // Reset double jump when grounded

                }
            }
            else if (playerModeValue == 1 || playerModeValue == 2)
            {
                // Use the moveInput vector to move your player
                Vector3 move = new Vector3(moveInput.x, moveInput.y, 0f);

                // Apply the movement to your player (e.g., transform position or rigidbody movement)
                transform.position += move * Time.deltaTime * flySpeed;
            }
            if (playerModeValue == 2)
            {
                RaycastHit2D hit = Physics2D.BoxCast(catchPoint.position, catchBoxSize, 0, Vector2.right, 0.1f, enemyLayer);


                if (hit.collider != null)
                {
                    hit.collider.gameObject.GetComponent<BeeEnemy>().ComponentDisble();
                    levelManager.UpdateDeadBees();
                    gameManager.PlaySfx("hit");
                    Pooler.Instance.ActiVfx("Hit", hit.collider.gameObject.transform);
                }

            }

        }
    }

  

    private void OnJoystickMove(InputAction.CallbackContext context)
    {
        if (isCanControl)
        {

            if (playerModeValue == 1 || playerModeValue == 2)
            {
                // Read the move input from the joystick
                moveInput = context.ReadValue<Vector2>();
            }
        }
      
    }

    private void OnStopTriger()
    {

        isCanControl = false;
        rb.velocity = Vector2.zero;
        transform.GetComponentInChildren<Collider2D>().enabled = false;
        rb.gravityScale = 0;

        if (playerModeValue == 0)
        {
            animator.SetInteger("PlayerState", 0);
        }
        StopAllCoroutines();
    }


    private void OnJump(InputAction.CallbackContext context)
    {
        if (isCanControl)
        {
            if (playerModeValue == 0)
            {

                if (context.performed)
                {
                    if (isGrounded || !isDoubleJump)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply jump force

                        if (!isGrounded)
                        {
                            isDoubleJump = true; // Set double jump flag
                        }

                        animator.SetTrigger("Jump");


                    }
                }
            }
        }
    }

    private void OnThrow(InputAction.CallbackContext context) 
    {
        switch (playerModeValue)
        {
            case 0:
                animator.SetTrigger("Throw");
                GameObject mobilel = Pooler.Instance.GetPooledObject("mobile");
                mobilel.transform.position = throwHand.position;

                mobilel.SetActive(true);
                Rigidbody2D mobileRb = mobilel.GetComponent<Rigidbody2D>();
                mobileRb.velocity = thorowForce;
                GameManager.Instance.PlaySfx("throw");

            break;

            case 1:
                GameObject bullet = Pooler.Instance.GetPooledObject("PBullet");
                bullet.transform.position = shootPos.position;

                bullet.SetActive(true);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                bulletRb.AddForce(Vector2.right*bullertForce);
                GameManager.Instance.PlaySfx("laser");

                break;
        }
     


    

    }
    private void OnFruitThrow(InputAction.CallbackContext context)
    {
        if (isCanControl)
        {
            if (levelManager.fruiteValue > 0)
            {
                GameObject fruit = Pooler.Instance.GetPooledObject("fruit");
                fruit.transform.position = throwHand.position;

                fruit.SetActive(true);
                Rigidbody2D fruitRb = fruit.GetComponent<Rigidbody2D>();
                fruitRb.velocity = thorowForce;
                levelManager.UpdateFruitValue(false);
                GameManager.Instance.PlaySfx("throw");
               
            }
        }

    }

    private void Hurt() 
    {
        if (isCanDamege)
        {
            levelManager.LifeDiscrease();
            Pooler.Instance.ActiVfx("Hit",transform);
            
        }
    }

    public void Relife() 
    {
        if (!isHitStatue)
        {
            rb.gravityScale = defaultGravityScale;
        }
     
        isCanControl = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            if (!isGrounded) 
            {
                animator.SetInteger("PlayerState", 1);
            }
        }

        if (collision.gameObject.CompareTag("statue"))
        {

            animator.SetTrigger("Hurt");
             isHitStatue = true;
            isCanControl =false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCanControl)
        {

            if (collision.CompareTag("Bee"))
            {
                gameManager.PlaySfx("hit");
                if (isFuelPoweUp)
                {
                    collision.GetComponent<BeeEnemy>().ComponentDisble();
                    levelManager.UpdateDeadBees();
                    Pooler.Instance.ActiVfx("Hit", collision.gameObject.transform);
                }
                Hurt();

            }

            else if (collision.CompareTag("coin"))
            {
                collision.GetComponent<ObjectsReset>().Deactive();
                gameManager.PlaySfx("coin");
                levelManager.UpdateCoinValue();
            }


            else if (collision.CompareTag("fruit"))
            {
                collision.GetComponent<ObjectsReset>().Deactive();
                gameManager.PlaySfx("fruit");
                levelManager.UpdateFruitValue(true);
            }

            else if (collision.CompareTag("Meat"))
            {
                collision.GetComponent<ObjectsReset>().Deactive();
                GameManager.Instance.PlaySfx("eat");
                levelManager.IsPlayerHasMeat = true;
                transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                StartCoroutine(DisbleMeatPowerUp());
                levelManager.MainSpeed = 7f;
                isCanDamege = false;
            }

            else if (collision.CompareTag("DmgBlock"))
            {
                gameManager.PlaySfx("hit");
                Hurt();
            }

            else if (collision.CompareTag("Fuel"))
            {
                if (!isFuelPoweUp)
                {

                    gameManager.PlaySfx("fuel");
                    isFuelPoweUp = true;
                    collision.GetComponent<FuelBarel>().ResetFuel();
                    isCanDamege = false;
                    levelManager.MainSpeed = 8;
                    StartCoroutine(DisbleFuelPowerUp());
                }
            }

            else if (collision.CompareTag("Portal")) 
            {
                Debug.Log("Portal");
                collision.transform.parent.GetComponent<Portal>().PortalReset();
                levelManager.TeleportCheck();
              
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("statue"))
        {
            if (isHitStatue) 
            {
                isHitStatue = false;
                rb.gravityScale = defaultGravityScale;
            }

           
        }
    }

    private void OnDisable()
    {
        inputControls.Disable();

        levelManager.OnGameStop.RemoveListener(OnStopTriger);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.DrawWireCube(catchPoint.position, catchBoxSize);
        }
    }


    IEnumerator DisbleMeatPowerUp() 
    {
        yield return new WaitForSeconds(10);
        transform.localScale = new Vector3(1, 1, 1);
        levelManager.IsPlayerHasMeat = false;
        levelManager.MainSpeed = 5f;
        isCanDamege = true;

    }

    IEnumerator DisbleFuelPowerUp() 
    {
        yield return new WaitForSeconds(10);
        levelManager.MainSpeed = 5f;
        isCanDamege = true;
        isFuelPoweUp = false;

    }
}

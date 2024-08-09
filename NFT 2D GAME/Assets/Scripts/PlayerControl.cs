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
    [SerializeField]Pooler pooler;

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

    //Delegate
    public delegate void IsMove(bool isMove); // Delegate definition
    public static event IsMove OnPlayerMove; // Event definition

    bool isCanControl;
    bool isHitStatue;

    float defaultGravityScale;

   

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

        levelManager.OnGameStop.AddListener(OnStopTriger);
    }

    private void OnStopTriger()
    {
       
        isCanControl = false;
        rb.velocity = Vector2.zero;
        transform.GetComponentInChildren<Collider2D>().isTrigger = false;
        rb.gravityScale = defaultGravityScale;
        animator.SetInteger("PlayerState", 0);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        switch (levelManager.playerMode) 
        {
            case LevelManager.PlayerMode.Ground:

                animator = playerSkins[0].GetComponent<Animator>();
                rb.gravityScale = 3;
                animator.SetInteger("PlayerState", 1);
                break;

            case LevelManager.PlayerMode.Sky:

                animator = playerSkins[1].GetComponent<Animator>();
                rb.gravityScale = 0;

                break;

            case LevelManager.PlayerMode.Space:

                animator = playerSkins[2].GetComponent<Animator>();
                rb.gravityScale = 0;

                break;
        }

       
        defaultGravityScale = rb.gravityScale;
        OnPlayerMove?.Invoke(true);
        isCanControl = true;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            isDoubleJump = false; // Reset double jump when grounded
           
        }

    }


    private void OnJump(InputAction.CallbackContext context)
    {
        if (isCanControl)
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

    private void OnThrow(InputAction.CallbackContext context) 
    {
        animator.SetTrigger("Throw");
        GameObject mobilel = pooler.GetPooledObject("mobile");
        mobilel.transform.position = throwHand.position; 

        mobilel.SetActive(true);
        Rigidbody2D mobileRb = mobilel.GetComponent<Rigidbody2D>(); 
        mobileRb.velocity = thorowForce;

    }
    private void OnFruitThrow(InputAction.CallbackContext context)
    {
        if (isCanControl)
        {
            if (levelManager.fruiteValue > 0)
            {
                GameObject fruit = pooler.GetPooledObject("fruit");
                fruit.transform.position = throwHand.position;

                fruit.SetActive(true);
                Rigidbody2D fruitRb = fruit.GetComponent<Rigidbody2D>();
                fruitRb.velocity = thorowForce;
                levelManager.UpdateFruitValue(false);
            }
        }

    }

    private void Hurt() 
    {
        isCanControl  = false;
        rb.gravityScale = 0;
        animator.SetTrigger("Hurt");
        OnPlayerMove?.Invoke(false);
        levelManager.LifeDiscrease();
       
    }

    public void Relife() 
    {
        if (!isHitStatue)
        {
            rb.gravityScale = defaultGravityScale;
        }
        OnPlayerMove?.Invoke(true);
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
            rb.gravityScale = 0;
            OnPlayerMove?.Invoke(false);
            animator.SetTrigger("Hurt");
             isHitStatue = true;
                GetComponentInChildren<Collider2D>().isTrigger = true;
            
            isCanControl =false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCanControl)
        {

            if (collision.CompareTag("Bee"))
            {
                levelManager.LifeDiscrease();
                gameManager.PlaySfx("hit");
                Hurt();

            }

            else if (collision.CompareTag("coin"))
            {
                collision.GetComponent<Coin>().Deactive();
                gameManager.PlaySfx("coin");              
                levelManager.UpdateCoinValue();
            }


            else if (collision.CompareTag("fruit"))
            {
                collision.GetComponent<FruitMove>().Deactive();
                gameManager.PlaySfx("fruit");

                levelManager.UpdateFruitValue(true);
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
                GetComponentInChildren<Collider2D>().isTrigger = false;
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
        }
    }
}

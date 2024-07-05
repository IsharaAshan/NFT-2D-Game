using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class PlayerOnFoot : MonoBehaviour
{
    Controls control;

    public float moveSpeed = 5f; // Speed at which the player moves
    public float jumpForce = 10f; // Force applied when the player jumps
    public Transform groundCheck; // Transform representing the ground check position
    public float groundCheckRadius = 0.2f; // Radius of the ground check circle
    public LayerMask groundLayer; // Layer mask for the ground
    public bool canMove;

    private Rigidbody2D rb;
    private bool isGrounded;
   Animator animator;

    LevelController levelController;

    short lifeValue;

    [SerializeField]Transform launchPos;
    [SerializeField] Vector2 throwForce;
    bool isPowerOn;
    short hitValue = 3;

    [SerializeField] GameObject[] activeSkin;

    public enum PlayerMode 
    {
        OnFoot,
        OnPlane,
        OnRocket
    }

    public PlayerMode playerMode;


    private void Awake()
    {
        control = new Controls();
        levelController = FindObjectOfType<LevelController>();
    }

    private void OnEnable()
    {
        control.Enable();
        control.Player.Shoot.performed += ctx => Shoot();
        control.Player.ThrowFruit.performed += ctx => ThrowFruit();  

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


        switch (playerMode) 
        {
                case PlayerMode.OnFoot:
                rb.gravityScale = 1;
                SkinActive(0);               
                animator = activeSkin[0].GetComponent<Animator>();
                ; break;

                case PlayerMode.OnPlane: SkinActive(1);
                  rb.gravityScale = 0;
                animator = activeSkin[1].GetComponent<Animator>();
                ; break;
                case PlayerMode.OnRocket: SkinActive(2);
                  rb.gravityScale = 0;
                animator = activeSkin[2].GetComponent<Animator>();
                ; break;
        }

       
    }

    private void Update()
    {
        // Check if the player is grounded
    
        switch (playerMode)
        {
            case PlayerMode.OnFoot:
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

                if (animator != null)
                {
                    animator.SetFloat("Speed", canMove ? moveSpeed : 0);
                    animator.SetBool("isJumping", !isGrounded);
                }

                if (canMove && isGrounded && Input.GetButtonDown("Jump"))
                {
                    Jump();
                }

                ; break;

           
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
        if (playerMode == PlayerMode.OnFoot)
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
            canMove = false;
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
       
        animator.SetTrigger("throw");

      
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
        transform.GetChild(0).localScale = new Vector3(.35f, .35f, .35f);
        StartCoroutine(DeactiveMeetPowerUp());
    }

    IEnumerator DeactiveMeetPowerUp() 
    {
        moveSpeed *= 2;
        yield return new WaitForSeconds(5);
        transform.GetChild(0).localScale = new Vector3(.25f, .25f, .25f);
        moveSpeed /= 2;
        isPowerOn = false;
        hitValue = 3;
    }


    public void SkinActive(short index) 
    {

        foreach (GameObject skin in activeSkin) 
        {
            skin.SetActive(false);
        }

        activeSkin[index].SetActive(true);
    }


    private void OnDrawGizmos()
    {
        // Draw a sphere at the ground check position for debugging purposes
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }


}

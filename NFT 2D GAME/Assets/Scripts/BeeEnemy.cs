using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody2D rb;
    LevelManager levelManager;

    [SerializeField] float endPostion;
    [SerializeField] float startPostionX;

    [SerializeField]bool isBig;
    private void Awake()
    {
       levelManager = FindObjectOfType<LevelManager>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        GameManager.Instance.OnMoveSpeedChange.AddListener(OnSpeedChange);

    }

    private void OnDisable()
    {
        GameManager.Instance.OnMoveSpeedChange.AddListener(OnSpeedChange);
    }



    private void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        if (transform.position.x <  endPostion) 
        {
            transform.position = new Vector2(startPostionX, 0);
            ComponentEnable();

        }
    }

    private void OnSpeedChange(float value)
    {
        moveSpeed = value;
    }



    private void ComponentDisble()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
    }

    private void ComponentEnable()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBig)
        {

            if (collision.CompareTag("mobile"))
            {
                collision.gameObject.SetActive(false);
                rb.isKinematic = false;
                levelManager.UpdateDeadBees();

                GameManager.Instance.PlaySfx("hit");
            }
        }
        else 
        {
            if (collision.CompareTag("ThrowFruit"))
            {
                collision.gameObject.SetActive(false);
                rb.isKinematic = false;
                levelManager.UpdateDeadBees();

                GameManager.Instance.PlaySfx("hit");
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
           
            ComponentDisble();
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }
}

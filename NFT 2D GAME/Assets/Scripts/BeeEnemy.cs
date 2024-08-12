using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEnemy : MonoBehaviour
{


    Rigidbody2D rb;
    LevelManager levelManager;

    [SerializeField] float endPostion;
    [SerializeField] float startPostionX;

    [SerializeField]bool isBig;

    [SerializeField]GameObject meat;
    Vector2 startPos;
    private void Awake()
    {
       levelManager = FindObjectOfType<LevelManager>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        startPos = transform.position;


    }


    private void Update()
    {
        float speed = levelManager.MainSpeed + 2;

        transform.Translate(Vector2.left * speed* Time.deltaTime);

        if (transform.position.x <  endPostion) 
        {
            
            transform.position = new Vector2(startPostionX, Random.Range(-2,2));
            ComponentEnable();

        }

       
    }




    public void ComponentDisble()
    {

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    private void ComponentEnable()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    private void DeadInstance() 
    {
        if (!levelManager.IsPlayerHasMeat) 
        {
            FindObjectOfType<ObjectsMover>().ActivePowerUpMeat(transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Pbullet"))
        {
            collision.gameObject.SetActive(false);
            GameManager.Instance.PlaySfx("hit");
            levelManager.UpdateDeadBees();
            ComponentDisble();
            Pooler.Instance.ActiVfx("Hit", transform);

        }
      


        if (!isBig)
        {

            if (collision.CompareTag("mobile"))
            {
                collision.gameObject.SetActive(false);
                rb.isKinematic = false;
                levelManager.UpdateDeadBees();

                ComponentDisble();
                GameManager.Instance.PlaySfx("hit");
                DeadInstance();

            }
        }
        else 
        {
            if (collision.CompareTag("ThrowFruit"))
            {
                collision.gameObject.SetActive(false);
                rb.isKinematic = false;
                levelManager.UpdateDeadBees();

                ComponentDisble();
                GameManager.Instance.PlaySfx("hit");
                DeadInstance();
            }
        }

    }

    public void AssignStartPostion()
    {
        transform.position = startPos;
    }




  
}

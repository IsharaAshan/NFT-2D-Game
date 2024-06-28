using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBee : MonoBehaviour
{
    [SerializeField] float speed;

    private void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ThrowObj"))
        {
            collision.gameObject.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
            GameManager.Instance.PlaySound("BeeHit");

            GetComponent<Rigidbody2D>().gravityScale = 1;

            GameObject poolObject = Pooler.Instance.GetPooledObject("meat");
            poolObject.transform.position = transform.position;
            poolObject.SetActive(true);
            gameObject.SetActive(false);

        }

        if (collision.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }

}

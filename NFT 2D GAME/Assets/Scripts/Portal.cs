using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    float endX = -12f;
    float startX = 12f;

    float speed = 5.0f;

    bool canMove;

    private void Start()
    {
        PortalReset();
    }

    private void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (transform.position.x <= endX)
        {

            PortalReset();

        }
    }

    public void PortalReset() 
    {
        StartCoroutine(RandomizePortal());
    }

    IEnumerator RandomizePortal() 
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        transform.position = new Vector2(startX, Random.Range(-3, 3));
        canMove = false;
        yield return new WaitForSeconds(Random.Range(5,8));
        canMove = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
    }


  
}

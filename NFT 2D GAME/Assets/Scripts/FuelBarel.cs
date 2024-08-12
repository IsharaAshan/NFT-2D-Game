using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBarel : MonoBehaviour
{
   float endX = -12f;
   float startX = 12f;

    float speed = 5.0f;

    bool isDeactive;


    private void Start()
    {
        ResetFuel();


    }

    // Update is called once per frame
    void Update()
    {
        if (!isDeactive)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (transform.position.x <= endX) 
        {
            Debug.Log("Acrive");
           
            if (!isDeactive) 
            {

                ResetFuel();

            }

        }
 
    }

    public void ResetFuel() 
    {
        StartCoroutine(RandomEnabel());
    }

    IEnumerator RandomEnabel() 
    {
        isDeactive = true;
        transform.position = new Vector2(startX, Random.Range(-4,4f));
        transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(Random.Range(2,5));
        transform.GetComponentInChildren<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        isDeactive=false;


        
    }
}

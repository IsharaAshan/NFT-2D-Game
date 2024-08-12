using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planets : MonoBehaviour
{
    float endX = -12f;
    float startX = 12f;

    float speed = 5.0f;


    Vector2 startPos;
    private void Start()
    {
        startPos = transform.position;
        StartCoroutine(RanDomstart());
    }
    private void Update()
    {


        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x <= endX)
        {

            StartCoroutine(RanDomstart());
        }

    }
    IEnumerator RanDomstart() 
    {
   
        transform.position = new Vector2(startX, Random.Range(-4, 4));
        yield return new WaitForSeconds(Random.Range(2,6));
      
    }

    public void AssignStartPostion() 
    {
        StartCoroutine(RanDomstart());
        transform.position = startPos;
    }


}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector2 moveRight = Vector2.right;
    [SerializeField] float speed;
    [SerializeField] float destoyTime = 1f;



    private void Start()
    {
        StartCoroutine(DestroyTimeCount());
    }

    private void Update()
    {
        transform.Translate(moveRight*speed*Time.deltaTime);
    }

    IEnumerator DestroyTimeCount() 
    {

        yield return new WaitForSeconds(destoyTime);
        gameObject.SetActive(false);
    }



}

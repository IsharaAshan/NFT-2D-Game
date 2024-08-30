using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    [SerializeField] bool isMobile;

    private void Start()
    {

        if (isMobile) 
        {

        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")) 
        {
            gameObject.SetActive(false);
        }
    }
}

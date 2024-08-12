using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsReset : MonoBehaviour
{
   

    public void Deactive() 
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    public void Active() 
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
}

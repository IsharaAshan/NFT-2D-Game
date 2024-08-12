using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avtar1 : MonoBehaviour
{
    [SerializeField]PlayerControl playerControl;


    public void Run() 
    {
        GameManager.Instance.PlaySfx("run");
    }

    public void Jump() 
    {
        GameManager.Instance.PlaySfx("jump");
        
    }

    public void Throw() 
    {
       
    }

    public void DamegeOver() 
    {
        playerControl.Relife();
       
    }

    public void RocketCatch() 
    {
       // Debug.Log("Catch");
    }
}

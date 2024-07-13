using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinAvata : MonoBehaviour
{
    [SerializeField]PlayerControl player;
    public void Throw() 
    {
        player.ThrwoConfigure();
    }

    public void WalikingSound() 
    {
        GameManager.Instance.PlaySound("WalkSound");
    }


    public void ActiveBeeCatch() 
    {
        player.IsHitCheckBee();
    }
}

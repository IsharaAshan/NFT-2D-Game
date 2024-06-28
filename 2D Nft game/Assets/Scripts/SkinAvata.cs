using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinAvata : MonoBehaviour
{
    [SerializeField]PlayerOnFoot player;
    public void Throw() 
    {
        player.ThrwoConfigure();
    }

    public void WalikingSound() 
    {
        GameManager.Instance.PlaySound("WalkSound");
    }
}

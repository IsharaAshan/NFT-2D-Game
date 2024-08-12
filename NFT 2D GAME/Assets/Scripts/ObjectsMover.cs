using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsMover : MonoBehaviour
{
 
    [SerializeField] float endXVaPosValue;
    [SerializeField] float StartXPosValue;

    LevelManager levelManager;

    public GameObject powerUpMeat;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

  

    private void Update()
    {
        transform.Translate(Vector2.left * levelManager.MainSpeed * Time.deltaTime);

        if (transform.position.x <= endXVaPosValue) 
        {

            transform.position = new Vector2(StartXPosValue, 0);

            foreach (Transform t in transform) 
            {

                if (t.GetComponentInChildren<ObjectsReset>() != null) 
                {
                    t.GetComponentInChildren<ObjectsReset>().Active();
                    if (powerUpMeat != null)
                    {

                        powerUpMeat.SetActive(false);
                    }
                }

               
            }
        }

    }


    public void ActivePowerUpMeat(Transform pos) 
    {
        powerUpMeat.transform.position = pos.position;
        powerUpMeat.SetActive(true);
    }
  

}

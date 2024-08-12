using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] float endPositionX;
    [SerializeField] float startPositionX;

    [SerializeField] float moveSpeed;

    [SerializeField]private bool canMove;

    LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

 

    private void Update()
    {

        
            transform.Translate(Vector3.left * levelManager.MainSpeed* Time.deltaTime);

            if (transform.position.x <= endPositionX)
            {
                transform.position = new Vector2(startPositionX, transform.position.y);
            }
        
    }

    private void OnSpeedChange(float value)
    {
        moveSpeed = value;
    }
}

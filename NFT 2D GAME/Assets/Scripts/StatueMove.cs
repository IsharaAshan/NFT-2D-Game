using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueMove : MonoBehaviour
{
    [SerializeField] float endPositionX;
    [SerializeField] float startPositionX;

    [SerializeField] float moveSpeed;

    [SerializeField] private bool canMove;


    private void OnEnable()
    {
        PlayerControl.OnPlayerMove += OnMove; // Subscribe to the event
        GameManager.Instance.OnMoveSpeedChange.AddListener(OnSpeedChange);
    }

    private void OnDisable()
    {
        PlayerControl.OnPlayerMove -= OnMove; // Unsubscribe from the event
        GameManager.Instance.OnMoveSpeedChange.RemoveListener(OnSpeedChange);
    }

    private void OnMove(bool isMove) // Handler method matching the delegate signature
    {
        canMove = isMove;
      
    }

    private void Update()
    {

        if (canMove)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

            if (transform.position.x <= endPositionX)
            {
                transform.position = new Vector2(startPositionX, transform.position.y);
            }
        }
    }

    private void OnSpeedChange(float value)
    {
        moveSpeed = value;
    }
}

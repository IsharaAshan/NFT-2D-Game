using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float endXValue;
    [SerializeField] float startXValue;


    private void OnEnable()
    {
      //  GameManager.Instance.OnMoveSpeedChange.AddListener(OnSpeedChange);
    }

    private void Start()
    {
        GameManager.Instance.OnMoveSpeedChange.AddListener(OnSpeedChange);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnMoveSpeedChange.RemoveListener(OnSpeedChange);
    }


    private void Update()
    {
       

        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        if (transform.position.x <= endXValue)
        {
            transform.position = new Vector2(startXValue, transform.position.y);
            Active();
        }
    }

    private void OnSpeedChange(float value)
    {
        moveSpeed = value;
    }

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

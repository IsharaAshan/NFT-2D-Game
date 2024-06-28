using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset = new Vector3(-2, 0, -10); // Offset from the player's position
    public float smoothSpeed = 0.125f; // Speed at which the camera will smooth its movement

    [SerializeField] float xValue;

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, -10), new Vector3(desiredPosition.x, transform.position.y, -10), smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}

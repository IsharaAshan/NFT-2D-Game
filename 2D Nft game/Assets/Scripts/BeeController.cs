using UnityEngine;

public class BeeController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float distanceInFront = 2f; // Distance in front of the player
    public float verticalAmplitude = 0.5f; // Amplitude of vertical movement
    public float verticalFrequency = 2f; // Frequency of vertical movement

    private float initialY;

    private void Start()
    {
        initialY = transform.position.y;
    }

    private void Update()
    {
        // Update the bee's position to be in front of the player at a specified distance
        Vector3 newPosition = player.position + player.right * distanceInFront;
        newPosition.y = initialY + Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        transform.position = newPosition;
    }
}

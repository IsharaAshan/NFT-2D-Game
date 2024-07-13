using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeepBounds : MonoBehaviour
{
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private Camera mainCamera;

    [SerializeField] GameObject avatar;

    void Start()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        objectWidth = avatar.transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = avatar.transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        transform.position = viewPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeepBounds : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

   [SerializeField] bool isKeepbounds;


    [SerializeField] SpriteRenderer[] skinAvtare; // 0 = plane; 1 = rocket

    public void SetUpAvatar(int modevlaue) 
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        if (modevlaue == 1) 
        {
            objectWidth = skinAvtare[0].bounds.extents.x; // Half the width of the object
            objectHeight = skinAvtare[0].bounds.extents.y; // Half the height of the object
        }
        else
        {
            objectWidth = skinAvtare[1].bounds.extents.x; // Half the width of the object
            objectHeight = skinAvtare[1].bounds.extents.y; // Half the height of the object
        }

        isKeepbounds = true;
      
    }

    void LateUpdate()
    {
        if (isKeepbounds)
        {

            Vector3 viewPos = transform.position;
            viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
            viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
            transform.position = viewPos;
        }
    }
}

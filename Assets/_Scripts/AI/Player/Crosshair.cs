using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public GameObject crosshairObject;
    private Vector3 targetPosition;

    //Get initial position and hide system cursor
    void Start()
    {
        targetPosition = crosshairObject.transform.position;
        Cursor.visible = false;
    }

    // Get mouse position and move crosshair object to that position
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        targetPosition.z = 0;

        crosshairObject.transform.position = targetPosition;
    }

}

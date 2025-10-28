using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public GameObject crosshairObject;
    private Vector3 targetPosition;
    //Get Crosshairs collider if it has one
    [SerializeField] private Collider2D crosshairCollider;

    //Reference to jumpscareScreen in UI
    [SerializeField] private GameObject jumpscareScreen;

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


    //JUST FOR FUN
    //if collider is triggered, show jumpscare screen for 1 second
    private void OnTriggerEnter2D(Collider2D collision)
    {
            jumpscareScreen.gameObject.SetActive(true);
            StartCoroutine(HideJumpscareAfterDelay(0.3f));
    }

    // Coroutine to hide jumpscare screen after a delay
    private IEnumerator HideJumpscareAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        jumpscareScreen.gameObject.SetActive(false);
    }

}

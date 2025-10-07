using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    private Vector2 pointerInput;

    [SerializeField]
    private InputActionReference meleeAttack, pointerPosition;

    private WeaponParent weaponParent;
    

    private void OnEnable()
    {
        meleeAttack.action.performed += PerformAttack;
    }

    private void OnDisable()
    {
        meleeAttack.action.performed -= PerformAttack;
    }

    private void PerformAttack(InputAction.CallbackContext obj)
    {
        weaponParent.MeleeAttack();
    }

    private void Awake()
    {
        weaponParent = GetComponentInChildren<WeaponParent>();
    }

    // Update is called once per frame
    void Update()
    {
        pointerInput = GetPointerInput();
        weaponParent.PointerPosition = pointerInput;

    }

    //Function gets mouse input from user to determine where character faces
    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}

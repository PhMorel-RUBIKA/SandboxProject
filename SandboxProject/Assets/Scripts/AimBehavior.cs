using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimBehavior : MonoBehaviour
{
    [SerializeField] private GameObject aimSpaceImage;
    [SerializeField] private GameObject aimPlacement;
    [SerializeField] private Vector2 range;

    public static AimBehavior instance;
    private Vector2 aimPosition;
    public Vector2 finalAimPos;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        aimSpaceImage.transform.localScale = new Vector3(range.x * 2 + 0.5f, range.y * 2 + 0.5f, 0);
    }

    private void Update()
    {
        finalAimPos = new Vector2(transform.position.x + aimPosition.x, transform.position.y + aimPosition.y);
        aimPlacement.transform.position = finalAimPos;
    }

    public void OnAimPerformed(InputAction.CallbackContext value)
    {
        aimPosition = value.ReadValue<Vector2>();
        aimPosition *= range;
        aimSpaceImage.gameObject.SetActive(true);
    }

    public void OnAimCancelled(InputAction.CallbackContext value)
    { 
        aimPosition = Vector2.zero;
        aimSpaceImage.gameObject.SetActive(false);
        aimPlacement.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}

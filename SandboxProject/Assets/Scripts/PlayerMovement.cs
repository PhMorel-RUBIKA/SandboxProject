using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float strengh = 0f;
    [SerializeField] private AnimationCurve movementCurve;

    public static PlayerMovement instance;
    private float time;
    private bool timePass;
    [HideInInspector] public NewControls _input = null;
    private Vector2 _moveVector = Vector2.zero;
    private Rigidbody _rb = null;

    private void Awake()
    {
        if (instance == null) instance = this;

        _input = new NewControls();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Movement.performed += OnMovementPerformed;
        _input.Player.Movement.canceled += OnMovementCancelled;
        _input.Menu.Restart.performed += OnRestartButton;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Movement.performed -= OnMovementPerformed;
        _input.Player.Movement.canceled -= OnMovementCancelled;
        _input.Menu.Restart.performed -= OnRestartButton;
    }

    private void Update()
    {
        if(timePass) time += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        moveSpeed = movementCurve.Evaluate(time);
        Vector3 moveForce = _moveVector * moveSpeed * strengh;
        _rb.AddForce(moveForce * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        _moveVector = value.ReadValue<Vector2>().normalized;
        timePass = true;
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        _moveVector = Vector2.zero;
        timePass = false;
        time = 0;
    }

    private void OnRestartButton(InputAction.CallbackContext value)
    {
        SceneManager.LoadScene(0);
    }
}

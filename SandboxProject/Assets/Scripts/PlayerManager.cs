using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerManager : MonoBehaviour
{
    #region PublicVar

    [Header("Player Movement Variables")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private AnimationCurve movementAccelerationCurve;

    [Header("Player Ability Variables")] [SerializeField]
    private AbilityData currentAbility;
    
    #endregion
    
    #region PrivateVar

    public static PlayerManager instance;
    private PlayerControls _input;
    private Rigidbody _rb;

    private bool _isTimeMovementPassing;
    private float _movementTimer;
    [HideInInspector] public Vector2 moveDirectionVector;
    private float _curveMoveSpeed;

    private bool _isPerformingAbility;
    
    #endregion

    #region InitializeFunction
    private void Awake()
    {
        if (instance == null) instance = this;

        _input = new PlayerControls();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Movement.performed += OnMovementPerformed;
        _input.Player.Movement.canceled += OnMovementCancelled;
        _input.Player.Ability.performed += OnAbilityPerformed;
    }
    
    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Movement.performed -= OnMovementPerformed;
        _input.Player.Movement.canceled -= OnMovementCancelled;
        _input.Player.Ability.performed -= OnAbilityPerformed;
    }

    #endregion

    private void Update()
    {
        TimerForMovementAcceleration();
        
    }

    private void FixedUpdate()
    {
        DoMovement();
    }

    #region MovementFunction

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveDirectionVector = value.ReadValue<Vector2>().normalized;
        _isTimeMovementPassing = true;
    }
    
    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveDirectionVector = Vector2.zero;
        _isTimeMovementPassing = false;
        _movementTimer = 0;
    }

    private void DoMovement()
    {
        _curveMoveSpeed = movementAccelerationCurve.Evaluate(_movementTimer);
        Vector3 moveForce = moveDirectionVector * _curveMoveSpeed * movementSpeed;
        _rb.AddForce(moveForce, ForceMode.Impulse);
    }

    private void TimerForMovementAcceleration()
    {
        if (_isTimeMovementPassing && _movementTimer < movementAccelerationCurve[movementAccelerationCurve.length - 1].time)
        {
            _movementTimer += Time.deltaTime;
        }
    }

    #endregion

    #region AbilityFunction

    private void OnAbilityPerformed(InputAction.CallbackContext value)
    {
        if (_isPerformingAbility) return;
        StartCoroutine(PerformAbility());
    }

    private IEnumerator PerformAbility()
    {
        _isPerformingAbility = true;
        currentAbility.ExecuteAbility();

        yield return new WaitForSeconds(currentAbility.abilityCooldown);
        
        _isPerformingAbility = false;
    }

    #endregion
}

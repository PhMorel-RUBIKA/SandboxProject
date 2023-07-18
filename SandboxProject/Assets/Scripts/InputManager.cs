using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private EnemyDetector enemyDetector;
    
    public static InputManager instance;
    public PlayerControls _input;

    private void Awake()
    {
        if (instance == null) instance = this;

        _input = new PlayerControls();
    }

    public void OnEnable()
    {
        _input.Enable();
        
        _input.Player.Movement.performed += OnJoystickLeftPerformed;
        _input.Player.Movement.canceled += playerManager.OnMovementCancelled;
        _input.Player.Ability.performed += playerManager.OnAbilityPerformed;

        _input.Player.WaterAttack.performed += OnWaterAttackRead;
        _input.Player.FireAttack.performed += OnFireAttackRead;
        _input.Player.ElectricityAttack.performed += OnElectricityAttackRead;
    }

    public void OnDisable()
    {
        _input.Disable();
        
        _input.Player.Movement.performed -= OnJoystickLeftPerformed;
        _input.Player.Movement.canceled -= playerManager.OnMovementCancelled;
        _input.Player.Ability.performed -= playerManager.OnAbilityPerformed;
        
        _input.Player.WaterAttack.performed -= OnWaterAttackRead;
        _input.Player.FireAttack.performed -= OnFireAttackRead;
        _input.Player.ElectricityAttack.performed -= OnElectricityAttackRead;
    }

    private void OnJoystickLeftPerformed(InputAction.CallbackContext value)
    {
        playerManager.OnMovementPerformed(value.ReadValue<Vector2>());
        enemyDetector.OnLeftJoystickRead(value.ReadValue<Vector2>());
    }

    private void OnFireAttackRead(InputAction.CallbackContext value)
    {
        playerManager.OnAttackPerformed(attackType.FIRE);
    }

    private void OnWaterAttackRead(InputAction.CallbackContext value)
    {
        playerManager.OnAttackPerformed(attackType.WATER);
    }

    private void OnElectricityAttackRead(InputAction.CallbackContext value)
    {
        playerManager.OnAttackPerformed(attackType.ELECTRICITY);
    }
}

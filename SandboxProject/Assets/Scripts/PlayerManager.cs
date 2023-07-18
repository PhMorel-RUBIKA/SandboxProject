using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    [Space]
    
    [Header("Player Ability Variables")] [SerializeField]
    private AbilityData currentAbility;

    [Space]
    
    [Header("Player Combat Variables")]
    [SerializeField] private bool isAttackingEnemy;
    [SerializeField] private EnemyDetector enemyDetector;
    [Space] 
    [SerializeField] public float smallDamage;
    [SerializeField] public float mediumDamage;
    [SerializeField] public float highDamage;
    [Space]
    [SerializeField] private float attackDuration;
    [SerializeField] private float offSetOfAttackRush;
    [SerializeField] private float attackKnockbackStrength;
    [SerializeField] private GameObject attackVFX;

    #endregion
    
    #region PrivateVar

    public static PlayerManager instance;
    private Rigidbody _rb;

    private bool _isTimeMovementPassing;
    private float _movementTimer;
    [HideInInspector] public Vector2 moveDirectionVector;
    private float _curveMoveSpeed;

    private bool _isPerformingAbility;

    private EnemyScript _lockedTarget;
    private Coroutine _attackCoroutine;
    
    #endregion

    #region InitializeFunction
    private void Awake()
    {
        if (instance == null) instance = this;
        
        _rb = GetComponent<Rigidbody>();
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

    public void OnMovementPerformed(Vector2 value)
    {
        moveDirectionVector = value.normalized;
        _isTimeMovementPassing = true;
    }
    
    public void OnMovementCancelled(InputAction.CallbackContext value)
    {
        CancelMovement();
    }

    private void DoMovement()
    {
        _curveMoveSpeed = movementAccelerationCurve.Evaluate(_movementTimer);
        Vector3 moveForce = moveDirectionVector * _curveMoveSpeed * movementSpeed;
        _rb.AddForce(moveForce, ForceMode.Impulse);
    }

    private void CancelMovement()
    {
        moveDirectionVector = Vector2.zero;
        _isTimeMovementPassing = false;
        _movementTimer = 0;
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

    public void OnAbilityPerformed(InputAction.CallbackContext value)
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

    #region AttackFunction

    public void OnAttackPerformed(attackType type)
    {
        if (isAttackingEnemy) return;
        
        if (enemyDetector.currentTarget == null) return;

        if (enemyDetector.InputMagnitude() > 0.2f)
            _lockedTarget = enemyDetector.CurrentTarget();
        
        Attack(_lockedTarget, type);
    }

    private void Attack(EnemyScript target, attackType type)
    {
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        _attackCoroutine = StartCoroutine(AttackCoroutine(attackDuration));

        if (target == null) return;
        
        MoveTowardTarget(target, attackDuration);

        IEnumerator AttackCoroutine(float duration)
        {
            CancelMovement();
            isAttackingEnemy = true;
            
            yield return new WaitForSeconds(duration);
            
            InputManager.instance.OnDisable();
            target.TakeDamage(type);
            DoKnockback(target);
            Instantiate(attackVFX, transform.position, Quaternion.identity);
            isAttackingEnemy = false;
            
            yield return new WaitForSeconds(.05f);
            
            InputManager.instance.OnEnable();
        }
    }

    private void DoKnockback(EnemyScript target)
    {
        var knockbackDirection = (target.transform.position - transform.position).normalized;
        var targetRb = target.GetComponent<Rigidbody>();
        
        targetRb.AddForce(knockbackDirection * attackKnockbackStrength, ForceMode.Impulse);
    }

    private void MoveTowardTarget(EnemyScript target, float duration)
    {
        transform.DOMove(TargetOffset(target.transform), duration);
    }

    private float TargetDistance(EnemyScript target)
    {
        return Vector2.Distance(transform.position, target.transform.position);
    }

    private Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, offSetOfAttackRush);
    }

    #endregion
}

public enum attackType
{
    WATER,
    ELECTRICITY,
    FIRE
}

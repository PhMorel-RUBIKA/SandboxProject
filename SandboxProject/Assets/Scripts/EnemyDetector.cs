using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask layerMask;

    public static EnemyDetector instance;
    private Vector2 _inputDirection;
    public EnemyScript currentTarget;
    private bool _gizmoToggle;

    private void Awake()
    {
        if (instance == null) instance = this;
        _inputDirection = Vector2.zero;
    }

    private void Update()
    {
        RaycastHit info;

        if (!Physics.SphereCast(transform.position, attackRadius, _inputDirection, out info, attackRange, layerMask)) return;
        if (info.collider.transform.GetComponent<EnemyScript>().IsAttackable())
        {
            currentTarget = info.collider.transform.GetComponent<EnemyScript>();
        }
    }

    public void OnLeftJoystickRead(Vector2 value)
    {
        _inputDirection = value.normalized;
    }

    public EnemyScript CurrentTarget()
    {
        return currentTarget;
    }
    
    public float InputMagnitude()
    {
        return _inputDirection.magnitude;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, _inputDirection);
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        if (CurrentTarget() != null) Gizmos.DrawSphere(CurrentTarget().transform.position, .5f);
    }
}

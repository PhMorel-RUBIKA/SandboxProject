using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float minAttackVelocity;
    [SerializeField] private float minAttackColliderSize;
    [SerializeField] private float speedScaleLerp;
    [SerializeField] private GameObject attackCollider;
    public float velocityViewer;

    private Rigidbody _rb;
    private float _velocity;
    private bool _isAttacking;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _velocity = _rb.velocity.magnitude;

        if (velocityViewer < _velocity) velocityViewer = _velocity;

        if (_velocity >= minAttackVelocity)
        {
            float scale = (minAttackColliderSize * _velocity) / minAttackVelocity;
            Math.Round(scale, 1);
            Vector3 newScale = new Vector3(scale, scale, scale);

            attackCollider.transform.localScale =
                Vector3.Lerp(attackCollider.transform.localScale, newScale, speedScaleLerp);
        }
        
        if (_velocity > minAttackVelocity && !_isAttacking) _isAttacking = true;
        else if (_velocity <= minAttackVelocity && _isAttacking) StartCoroutine(TimerBeforeUpdate());

        var colliderEnabled = attackCollider.activeSelf;
        colliderEnabled = _isAttacking switch
        {
            false when colliderEnabled => false,
            true when !colliderEnabled => true,
            _ => colliderEnabled
        };
        attackCollider.SetActive(colliderEnabled);
    }

    IEnumerator TimerBeforeUpdate()
    {
        _isAttacking = false;
        yield return new WaitForSeconds(1);
        StopAttacking();
    }

    private void StopAttacking()
    {
        _isAttacking = false;
        ScoreManager.instance.AddFinalScore();
        AttackManager.instance.chainKill = 0;
        ScoreManager.instance.chainKill.text = 0.ToString();
    }
}

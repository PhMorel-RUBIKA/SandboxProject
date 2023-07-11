using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackColliderBehavior : MonoBehaviour
{
    [SerializeField] private string enemyTagName;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(enemyTagName)) return;
        AttackManager.instance.KillEnemy(other.gameObject);
    }
}

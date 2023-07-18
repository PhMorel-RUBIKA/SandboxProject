using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] public float health = 10;
    [SerializeField] public attackType type;
    [SerializeField] private GameObject damageVFX;
    [SerializeField] private CameraShake.Properties damageShake;
    [Space]
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private CameraShake.Properties deathShake;
    
    public void TakeDamage(attackType attackType)
    {
        var damage = 0f;

        damage = attackType switch
        {
            attackType.FIRE => type switch
            {
                attackType.FIRE => PlayerManager.instance.mediumDamage,
                attackType.WATER => PlayerManager.instance.smallDamage,
                attackType.ELECTRICITY => PlayerManager.instance.highDamage,
                _ => damage
            },
            attackType.WATER => type switch
            {
                attackType.FIRE => PlayerManager.instance.highDamage,
                attackType.WATER => PlayerManager.instance.mediumDamage,
                attackType.ELECTRICITY => PlayerManager.instance.smallDamage,
                _ => damage
            },
            attackType.ELECTRICITY => type switch
            {
                attackType.FIRE => PlayerManager.instance.smallDamage,
                attackType.WATER => PlayerManager.instance.highDamage,
                attackType.ELECTRICITY => PlayerManager.instance.mediumDamage,
                _ => damage
            },
            _ => PlayerManager.instance.mediumDamage
        };

        DoDamage(damage);
    }

    private void DoDamage(float damage)
    {
        health -= damage;
        CameraShake.instance.StartShake(damageShake);
        Instantiate(damageVFX, transform.position, Quaternion.identity);
        
        if (!IsAttackable()) DoDeath();
    }

    private void DoDeath()
    {
        CameraShake.instance.StartShake(deathShake);
        Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    public bool IsAttackable()
    {
        return health > 0;
    }
}

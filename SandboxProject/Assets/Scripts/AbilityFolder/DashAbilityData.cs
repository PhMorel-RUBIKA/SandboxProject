using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dash Ability", menuName = "Abilities/Dash Ability")]
public class DashAbilityData : AbilityData
{
    [SerializeField] private float dashDistance;

    public override void ExecuteAbility()
    {
        Vector3 originalPosition = PlayerManager.instance.transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dash Ability", menuName = "Abilities/Dash Ability")]
public class DashAbilityData : AbilityData
{
    [SerializeField] private float dashDistance;

    public override void ExecuteAbility()
    {
        Vector2 dashDirection = PlayerManager.instance.moveDirectionVector.normalized;
        PlayerManager.instance.GetComponent<Rigidbody>().AddForce(dashDirection * dashDistance, ForceMode.Impulse);
    }
}

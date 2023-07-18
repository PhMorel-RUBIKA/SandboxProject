using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Slash Attack", menuName = "Attack/Slash")]
public class SlasherAttackData : AttackData
{
    public override void ExecuteAttack()
    {
       Debug.Log("Je fais un slash"); 
    }
}

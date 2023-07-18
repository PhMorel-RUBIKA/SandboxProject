using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackData : ScriptableObject
{
   [SerializeField] private string nomAttaque;

   public abstract void ExecuteAttack();
}

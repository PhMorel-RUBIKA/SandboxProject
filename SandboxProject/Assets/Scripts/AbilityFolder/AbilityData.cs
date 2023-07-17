using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    [SerializeField] private float abilityDuration;
    public abstract void ExecuteAbility();
}

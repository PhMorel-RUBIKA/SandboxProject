using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    public float abilityCooldown;
    public abstract void ExecuteAbility();
}

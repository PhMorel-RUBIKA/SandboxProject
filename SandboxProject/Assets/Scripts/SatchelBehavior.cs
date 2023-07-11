using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatchelBehavior : MonoBehaviour
{
    [SerializeField] private float satchelTimeToExplode;
    public int id;
    
    private void OnEnable()
    {
        StartCoroutine(ExplosionTime());
    }

    IEnumerator ExplosionTime()
    {
        yield return new WaitForSeconds(satchelTimeToExplode); 
        SatchelManager.instance.ExplodeSatchel(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int probabilityOfSecondEnemy;
    [SerializeField] public Color basicColor;
    [SerializeField] public Color secondColor;

    public int life;

    private void OnEnable()
    {
        int value = Random.Range(0, 100);
        if (value >= probabilityOfSecondEnemy)
        {
            gameObject.GetComponent<SpriteRenderer>().color = basicColor;
            life = 1;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = secondColor;
            life = 2;
        }
    }
}

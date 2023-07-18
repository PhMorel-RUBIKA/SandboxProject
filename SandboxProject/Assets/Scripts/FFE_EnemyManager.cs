using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FFE_EnemyManager : MonoBehaviour
{
    [SerializeField] private int numberToSpawn;
    [SerializeField] private GameObject prefabToSpawn;
    [Space]
    [SerializeField] private Color waterEnemy;
    [SerializeField] private Color fireEnemy;
    [SerializeField] private Color electricityEnemy;

    private GameObject _currentInstance;
    private List<GameObject> _enemySpawned = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            _enemySpawned.Add(null);
            SpawnPrefab(i);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _enemySpawned.Count; i++)
        {
            if (_enemySpawned[i] == null) SpawnPrefab(i);
        }
    }

    private void SpawnPrefab(int id)
    {
        int positionIndex = Random.Range(0, transform.childCount);
        Transform spawnPosition = transform.GetChild(positionIndex);

        _currentInstance = Instantiate(prefabToSpawn, spawnPosition.position, spawnPosition.rotation);
        _enemySpawned[id] = _currentInstance;

        var rand = Random.Range(1, 4);
        switch (rand)
        {
            case 1 :
                _currentInstance.GetComponent<EnemyScript>().type = attackType.WATER;
                _currentInstance.GetComponent<SpriteRenderer>().color = waterEnemy;
                break;
            case 2 :
                _currentInstance.GetComponent<EnemyScript>().type = attackType.FIRE;
                _currentInstance.GetComponent<SpriteRenderer>().color = fireEnemy;
                break;
            case 3 :
                _currentInstance.GetComponent<EnemyScript>().type = attackType.ELECTRICITY;
                _currentInstance.GetComponent<SpriteRenderer>().color = electricityEnemy;
                break;
        }
    }
}

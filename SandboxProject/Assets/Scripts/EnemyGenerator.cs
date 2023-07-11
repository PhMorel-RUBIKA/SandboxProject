using System.Collections.Generic;
using TMPro;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PrefabGenerator : MonoBehaviour
{
    [SerializeField] private GameObject prefabsToSpawn;
    [SerializeField] private float numberToSpawnStart;
    [SerializeField] private float enemyLengthWave;
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private float tutorialLength;

    private GameObject _currentInstance;
    private float _currentWave;
    private List<GameObject> _enemySpawned = new List<GameObject>();

    void Start()
    {
        _currentWave = enemyLengthWave + numberToSpawnStart;
        waveNumberText.text = enemyLengthWave.ToString();
        
        StartCoroutine(Tutorial());
    }

    void SpawnPrefab(int id)
    {
        int positionIndex = Random.Range(0, transform.childCount);
        Transform spawnPosition = transform.GetChild(positionIndex);

        _currentInstance = Instantiate(prefabsToSpawn, spawnPosition.position, spawnPosition.rotation);
        _enemySpawned[id] = _currentInstance;
        
        _currentWave--;
        waveNumberText.text = _currentWave.ToString();
    }

    void Update()
    {
        for (int i = 0; i < _enemySpawned.Count; i++)
        {
            if (_enemySpawned[i] == null && _currentWave > 0) SpawnPrefab(i);
        }
        
        if (_currentWave > 0) return;
        StopGame();
    }
    
    private void StopGame()
    {
        endGameCanvas.SetActive(true);
        PlayerMovement.instance._input.Player.Disable();
    }

    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(tutorialLength);

        tutorialUI.SetActive(false);

        for (int i = 0; i < numberToSpawnStart; i++)
        {
            _enemySpawned.Add(null);
            SpawnPrefab(i);
        }
    }
}
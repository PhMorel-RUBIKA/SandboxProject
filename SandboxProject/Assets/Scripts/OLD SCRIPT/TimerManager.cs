using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float gameLength;
    [SerializeField] private GameObject endGameCanvas;

    private float _timer;

    private void Start()
    {
        _timer = gameLength;
        timerText.text = _timer.ToString("00");
    }

    private void Update()
    {
        if (_timer >= 0) _timer -= Time.deltaTime;
        timerText.text = _timer.ToString("00");

        if (_timer <= 0) StopGame();
    }

    private void StopGame()
    {
        endGameCanvas.SetActive(true);
        PlayerMovement.instance._input.Disable();
    }
}

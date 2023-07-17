using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreGlobal;
    [SerializeField] private TextMeshProUGUI addScore;
    [SerializeField] public TextMeshProUGUI chainKill;
    public int scoreToAddPerEnemy;

    public static ScoreManager instance;
    private int _currentScore;
    [HideInInspector] public int lastScore;
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        _currentScore = 0;
        lastScore = 0;
        
        addScore.text = lastScore.ToString("D3");
        scoreGlobal.text = _currentScore.ToString("D4");
    }

    public void AddCurrentScore(int score, int chain)
    {
        lastScore += score;
        chainKill.text = chain.ToString("D1");
        addScore.text = lastScore.ToString("D3");
    }

    public void AddFinalScore()
    {
        _currentScore += lastScore;
        lastScore = 0;

        scoreGlobal.text = _currentScore.ToString("D4");
        addScore.text = lastScore.ToString("D3");
    }
}

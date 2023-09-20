using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Virtual Score", menuName = "Score System/Virtual Score SO")]
public class VirtualScoreSO : ScriptableObject
{
    [SerializeField] private int _score;
    public Action<int, int> OnUpdateScore;

    public void AddScore(int value)
    {
        _score += value;
        if (_score < 0)
            _score = 0;

        OnUpdateScore?.Invoke(_score, value);
    }

    public int GetScore()
    {
        return _score;
    }

    public string GetScoreString()
    {
        return _score.ToString();
    }

    public void UpdateScore()
    {
        OnUpdateScore?.Invoke(_score, 0);
    }

    public void SetScore(int value)
    {
        _score = value;
        OnUpdateScore?.Invoke(_score, value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private VirtualScoreSO _score;
    [SerializeField] private int _pointsToWin;
    private void Start()
    {
        _score.SetScore(0);
    }
    private void OnEnable()
    {
        _score.OnUpdateScore += VerifyScore;
    }
    private void OnDisable()
    {
        _score.OnUpdateScore -= VerifyScore;
    }
    private void VerifyScore(int x, int y)
    {
        if (x >= _pointsToWin)
            GameManager.Instance.Win();
    }
}

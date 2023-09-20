using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowScoreUI : MonoBehaviour
{
    [SerializeField] private VirtualScoreSO _score;
    [SerializeField] private TextMeshProUGUI _addText;
    [SerializeField] private bool _persistentAddText = false;
    [SerializeField] private TextMeshProUGUI _text;
    private CanvasGroup _addTextCanvas;
    private int _currentAddScore;
    private void Awake()
    {
        if (_addText && _addText.gameObject.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
        {
            _addTextCanvas = canvasGroup;
        }
    }

    private void Start()
    {
        _text.text = _score.GetScoreString();
    }

    private void OnEnable()
    {
        _text.text = _score.GetScoreString();
        _score.OnUpdateScore += UpdateText;
    }
    private void OnDisable()
    {
        _score.OnUpdateScore -= UpdateText;

        _currentAddScore = 0;
    }
    private void UpdateText(int totalScores, int addScore)
    {
        if (addScore > 0)
        {
            if (_addTextCanvas != null) LeanTween.cancel(_addTextCanvas.gameObject);

            _currentAddScore += addScore;

            if (_addTextCanvas != null) _addTextCanvas.alpha = 1;
            _addText.text = "+" + _currentAddScore.ToString();

            if (!_persistentAddText)
            {
                LeanTween.alphaCanvas(_addTextCanvas, 0, 1.5f).setEaseInBack().setOnComplete(() => {
                    _currentAddScore = 0;
                });
            }
        }

        _text.text = totalScores.ToString();
    }
}

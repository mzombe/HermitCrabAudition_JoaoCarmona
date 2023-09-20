using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public enum GameStyle
{
    STYLE3D,
    STYLE2D
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameEventSO _onGameEventWin;
    [SerializeField] private GameEventSO _onGameEventLose;
    private GameStyle _currentStyle {
        get{
            return m_Settings.CurrentStyle;
        }
    }
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(GameManager).Name);
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }
            //DontDestroyOnLoad(_instance.gameObject);

            return _instance;
        }
    }
    [Inject]
    public Settings m_Settings {get; private set;}
    public GameManager(Settings settings)
    {
        m_Settings = settings;
    }

    private void Awake() => _instance = this;
    public static Action<GameStyle> OnChangeGameStyle;
    
    private void Start() {
        SetGameStyle(_currentStyle);
        Application.targetFrameRate = 60;
    }
    public GameStyle GetCurrentStyle()
    {
        return _currentStyle;
    }
    public void SetGameStyle(GameStyle style)
    {
        m_Settings.CurrentStyle = style;
        OnChangeGameStyle?.Invoke(_currentStyle);
    }

    public void ToogleStyle()
    {
        if(_currentStyle == GameStyle.STYLE3D)
            SetGameStyle(GameStyle.STYLE2D);
        else if (_currentStyle == GameStyle.STYLE2D)
            SetGameStyle(GameStyle.STYLE3D);
    }
    public void Lose()
    {
        _onGameEventLose?.Invoke();
    }
    public void Win()
    {
        _onGameEventWin?.Invoke();
    }
    [Serializable]
    public class Settings{
        public GameStyle CurrentStyle;
    }
}

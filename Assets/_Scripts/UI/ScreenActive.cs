using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenActive : MonoBehaviour
{
    [SerializeField] private GameObject _screen;
    [SerializeField] private GameEventSO _onGameEvent;
    private void Start()
    {
        if (_screen)
            _screen = transform.GetChild(0).gameObject;
    }
    private void OnEnable()
    {
        _onGameEvent.Subcribe(ActiveScreen);
    }
    private void OnDisable()
    {
        _onGameEvent.Unsubcribe(ActiveScreen);
    }
    private void ActiveScreen()
    {
        _screen.SetActive(true);
    }
}

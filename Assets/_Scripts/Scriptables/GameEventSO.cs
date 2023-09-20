using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Event")]
public class GameEventSO : ScriptableObject
{
    private Action action = delegate { };
    public void Invoke() => action();
    public void Subcribe(Action method) => action += method;
    public void Unsubcribe(Action method) => action -= method;

}
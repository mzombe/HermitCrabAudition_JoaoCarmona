using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTriggerEvent : MonoBehaviour
{
    public UnityEvent _use;
    public UnityEvent _altUse;
    public void Use()
    {
        _use.Invoke();
    }

    public void AltUse()
    {
        _altUse.Invoke();
    }
}

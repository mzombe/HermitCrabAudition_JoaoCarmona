using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMove : MonoBehaviour
{
    [SerializeField] private float _timeToRoam;
    [SerializeField] private float _distance;

    private void Start()
    {
        LeanTween.move(gameObject, transform.position + transform.right * _distance, _timeToRoam).setLoopPingPong();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * _distance);
    }
}

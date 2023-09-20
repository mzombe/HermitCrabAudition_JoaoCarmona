using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamage>(out IDamage damage))
        {
            damage.Dead();
        }
    }
}

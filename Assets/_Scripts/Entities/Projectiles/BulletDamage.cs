using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [SerializeField] private float _damage;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemy)){
            enemy.TakeDamage(_damage);

            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _timeToDestroy;
    [SerializeField] private float _damage;
    // [Header("VFX")]
    // [SerializeField] private GameObject _vfx;

    public float Damage => _damage;

    private void Start() {
        Destroy(gameObject, _timeToDestroy);

        if(transform.GetChild(0).TryGetComponent<ParticleSystemRenderer>(out ParticleSystemRenderer particle)){
            particle.flip = new Vector3(transform.rotation.eulerAngles.y > 90 ? 1 : -1,0,0);
        }
    }


   void Update(){
        transform.position += transform.forward * Time.deltaTime * _projectileSpeed;
    }

    private void OnTriggerEnter(Collider other) {
        Particles(other.gameObject);
    }

    private void OnCollisionEnter(Collision other) {
        Particles(other.gameObject); 
    }

    private void Particles(GameObject other){
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : Player
{
    [SerializeField] private float _rayCastSize;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _outRight;
    [SerializeField] private Transform _outLeft;
    [SerializeField] private float _cadenceBullet;
    [SerializeField] private GameObject _prefabBullet;
    [Header("VFX")]
    [SerializeField] private GameObject _vfxMuzzle;

    private bool _isAim = false;
    private bool _isShooting = false;
    private List<GameObject> _enemiesInRange = new List<GameObject>();
    public PlayerShoot(Settings settings) : base(settings){
        
    }
    private void Start() {
        _enemiesInRange = new List<GameObject>();
    }
    private void Update() {
        _player.PlayerAnimator.SetBool("isShooting", _isAim);

        if(_enemiesInRange.Count > 0 && !_isAim)
        {
            _isAim = true;

            _player.PlayerAnimator.SetTrigger("Shoot");
            StartCoroutine(FireDelay());
        }else{
            for (int i = 0; i < _enemiesInRange.Count; i++){
                if(!_enemiesInRange[i] || !_enemiesInRange[i].activeSelf)
                    _enemiesInRange.Remove(_enemiesInRange[i]);
                if(_enemiesInRange.Count == 0)
                    _isAim = false;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == 7 && !_enemiesInRange.Contains(other.gameObject))
            _enemiesInRange.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == 7 && _enemiesInRange.Contains(other.gameObject))
            _enemiesInRange.Remove(other.gameObject);

        if(_enemiesInRange.Count == 0)
            _isAim = false;
    }

    private IEnumerator FireDelay(){
        if(_isShooting)
            yield break;

        while (_isAim){
            _isShooting = true;

            yield return new WaitForSeconds(_cadenceBullet);
            if (_isAim)
                Fire();
        }

        _isShooting = false;
    }

    private void Fire(){
        _player.PlayerAnimator.SetBool("isShooting", true);

        if(_player.PlayerMove.IsRight){
            Shoot(_outRight);
        }else{
            Shoot(_outLeft);
        }
    }

    private void Shoot(Transform target){
        if(_vfxMuzzle){
            GameObject vfx = Instantiate(_vfxMuzzle, target.position, target.rotation);
            vfx.transform.SetParent(transform);

            Destroy(vfx, 2f);
        }

        Instantiate(_prefabBullet, target.position, target.rotation);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        if(_isAim)
            Gizmos.color = Color.red;

        if(gameObject.TryGetComponent<SphereCollider>(out SphereCollider sphere))
            Gizmos.DrawWireSphere(transform.position, sphere.radius);
    }
}

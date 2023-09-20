using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _rayDistance;
    [SerializeField] private float _speed;
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float size;
    [Range(0f, 0.5f)]
    [SerializeField] private float distanceBetween;
    [SerializeField] private Vector3 position;
 
    private bool _isRight = false;
    private Vector3 _targetPosition;
    private Transform _playerTransform;
    private Vector3 _moveDir;
    private float _ySpeed;
    private Vector3 impact = Vector3.zero;
    private RaycastHit _hit;
    private Enemy _enemy;
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        StartCoroutine(ResetDir());
    }
    private void TurnRight(){
        _isRight = true;
        if(GameManager.Instance.GetCurrentStyle() == GameStyle.STYLE3D)
            LeanTween.rotateY(_enemy.Visual, 0, 0.2f);
        else if (GameManager.Instance.GetCurrentStyle() == GameStyle.STYLE2D)
            _enemy.Visual.transform.rotation = Quaternion.Euler(0, 0f, 0);
    }
    private void TurnLeft(){
        _isRight = false;
        if(GameManager.Instance.GetCurrentStyle() == GameStyle.STYLE3D)
            LeanTween.rotateY(_enemy.Visual, -180, 0.2f);
        else if (GameManager.Instance.GetCurrentStyle() == GameStyle.STYLE2D)
            _enemy.Visual.transform.rotation = Quaternion.Euler(0, -180f, 0);
    }
    private void Update(){
        if (!GroundCheck()){
            Gravity();
            return;
        }

        _enemy.EnemyAnimator.SetBool("isRun", _enemy.CharacterController.velocity.magnitude > 0.1f);
        if (_moveDir.x > 0)
            TurnRight();
        else
            TurnLeft();

        if (_playerTransform)
            _moveDir = _playerTransform.position - transform.position;

        _moveDir.Normalize();
        _enemy.CharacterController.Move(new Vector3(_playerTransform ? _moveDir.x : AvoidDir().x , 0f, 0f) * _speed * Time.deltaTime );
    }
    private Vector3 AvoidDir()
    {
        Vector3 dir = _moveDir;

        if (Physics.Raycast(transform.position, -transform.right, out _hit, _rayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3.left * _rayDistance), UnityEngine.Color.red);
            dir = transform.right;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3.left * _rayDistance), UnityEngine.Color.green);
        }

        if (Physics.Raycast(transform.position, transform.right, out _hit, _rayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3.right * _rayDistance), UnityEngine.Color.red);
            dir = -transform.right;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3.right * _rayDistance), UnityEngine.Color.green);
        }

        _moveDir = dir;
        return dir;
    }
    private bool GroundCheck()
    {
        if (Physics.Raycast(new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z), -transform.up, size, groundLayer) ||
            Physics.Raycast(new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z) + new Vector3(distanceBetween, 0, 0), -transform.up, size, groundLayer) ||
            Physics.Raycast(new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z) + new Vector3(-distanceBetween, 0, 0), -transform.up, size, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Gravity()
    {
        _ySpeed += !GroundCheck() ? Physics.gravity.y * Time.deltaTime : 0;

        if (impact.magnitude > 0.2)
        {
            _enemy.CharacterController.Move(impact * Time.deltaTime);
            impact = Vector3.Lerp(impact, Vector3.zero, -Physics.gravity.y * Time.deltaTime);
        }
        else
        {
            impact = Vector3.zero;
            _enemy.CharacterController.Move(new Vector3(0, _ySpeed, 0) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.TryGetComponent<Player>(out Player player))
            _playerTransform = player.gameObject.transform;
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.TryGetComponent<Player>(out Player player))
            _playerTransform = null;
    }
    private IEnumerator ResetDir(){
        yield return new WaitForSeconds(_delay);

        int x = UnityEngine.Random.Range(-1, 1);
        if (x == 0)
            _moveDir = transform.right;
        else
            _moveDir = -transform.right;

        StartCoroutine(ResetDir());
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z) + new Vector3(distanceBetween, 0, 0), -transform.up * size);
        Gizmos.DrawRay(new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z), -transform.up * size);
        Gizmos.DrawRay(new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z) + new Vector3(-distanceBetween, 0, 0), -transform.up * size);
    }
    public bool CanAttackMelee()
    {
        if (_playerTransform && _playerTransform.position.x - transform.position.x > -1.15f && _enemy.CharacterController.velocity.magnitude == 0)
            return true;
        else
            return false;
    }
}

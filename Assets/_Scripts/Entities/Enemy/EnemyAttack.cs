using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _damage;
    private Enemy _enemy;
    private bool _isAttacking;
    private bool _canAttack;
    private PlayerHealth _player;
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }
    private void OnDisable()
    {
        _enemy.EnemyAnimator.SetBool("isAttack", false);
        _player = null;
    }
    private void Update()
    {
        _canAttack = _enemy.EnemyMove.CanAttackMelee();
        _enemy.EnemyAnimator.SetBool("isAttack", _canAttack);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            _player = player;
        }
    }
    public void CauseDamage()
    {
        if (!_canAttack)
            return;

        if (_player)
            _player.TakeDamage(_damage);
    }
}

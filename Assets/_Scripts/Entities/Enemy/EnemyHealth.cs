using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamage
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private int _valueScore;
    [SerializeField] private VirtualScoreSO _score;
    private float _currentHealth;
    private bool _isLoopingEffect;
    private Enemy _enemy;
    private void Awake()
    {
        _enemy = gameObject.GetComponent<Enemy>();
    }
    private void Start() {
        _currentHealth = _maxHealth;
    }
    private bool Damage(float damage){
        if(_currentHealth > 0){
            _currentHealth -= damage;

            if(_currentHealth <= 0)
                return false;
            return true;
        }

        return false;
    }
    public void Dead()
    {
        EnemyManager.Instance.Remove(gameObject);
    }
    public void TakeDamage(float damage){
        if (!_isLoopingEffect) {
            _isLoopingEffect = true;
            LeanTween.alpha(_enemy.Visual, 0, 0.1f).setLoopPingPong(1).setOnComplete(() =>
            {
                _isLoopingEffect = false;
            });
        }

        if (!Damage(damage))
        {
            _score.AddScore(_valueScore);
            Dead();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class PlayerHealth : Player, IDamage
{
    [SerializeField] private Image _sprLife;
    public float _health {get{
        return _player.m_Settings.Health;
    }}
    private float _currentHealth;
    private bool _isLoopingEffect;
    private bool _barIsVisible;
    private CanvasGroup _bar;
    public PlayerHealth(Settings settings) : base(settings)
    {

    }

    private void Start()
    {
        _currentHealth = _health;
        _bar = _sprLife.gameObject.GetComponent<CanvasGroup>();
    }
    private bool Damage(float damage)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
                return false;
            return true;
        }

        return false;
    }
    public void Dead()
    {
        GameManager.Instance.Lose();
        gameObject.SetActive(false);
    }
    public void TakeDamage(float damage)
    {
        if (!_isLoopingEffect)
        {
            _isLoopingEffect = true;
            LeanTween.alpha(_player.Visual, 0, 0.1f).setLoopPingPong(1).setOnComplete(() =>
            {
                _isLoopingEffect = false;
            });
        }

        if (!Damage(damage))
            Dead();

        LifeBar();
    }

    private void LifeBar()
    {
        _sprLife.fillAmount = _currentHealth / _health;

        _bar.alpha = 1;
        if (!_barIsVisible)
        {
            LeanTween.alphaCanvas(_bar, 0f, 2f).setOnComplete(() => {
                _barIsVisible = false;
            });
            _barIsVisible = true;
        }
    }
}

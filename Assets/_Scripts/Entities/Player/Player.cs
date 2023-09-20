using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class Player : MonoBehaviour
{
    public Animator PlayerAnimator {get; private set;}
    public CharacterController PlayerController {get; private set;}
    public GameObject Visual {get; private set;}
    public PlayerMove PlayerMove {get; private set;}
    public  PlayerHealth PlayerHealth {get; private set;}
    protected static Player _player {get; private set;}

    [Inject]
    public Settings m_Settings {get; private set;}
    public Player(Settings settings)
    {
        m_Settings = settings;
    }

    private void Awake()
    {
        _player = this;

        PlayerAnimator = gameObject.GetComponentInChildren<Animator>();
        PlayerController = gameObject.GetComponent<CharacterController>();

        PlayerMove = gameObject.GetComponent<PlayerMove>();
        PlayerHealth = gameObject.GetComponent<PlayerHealth>();

        Visual = PlayerAnimator.gameObject;
    }

    [Serializable]
    public class Settings
    {
        public float Health;
        public float Speed;
        public float JumpPower;
    }

}

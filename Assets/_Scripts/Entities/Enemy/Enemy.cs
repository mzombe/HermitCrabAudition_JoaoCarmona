using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator EnemyAnimator {get; private set;}
    public GameObject Visual {get; private set;}
    public Rigidbody Rigidbody { get; private set;}
    public CharacterController CharacterController { get; private set;} 
    public EnemyAttack EnemyAttack { get; private set;}
    public EnemyMove EnemyMove { get; private set;}

    protected static Enemy _enemy;

    private void Awake() {
        _enemy = this;
        
        EnemyAnimator = gameObject.GetComponentInChildren<Animator>();
        Rigidbody = gameObject.GetComponent<Rigidbody>();
        CharacterController = gameObject.GetComponent<CharacterController>();
        EnemyAttack = gameObject.GetComponent<EnemyAttack>();
        EnemyMove = gameObject.GetComponent<EnemyMove>();

        Visual = EnemyAnimator.gameObject;
    }

}

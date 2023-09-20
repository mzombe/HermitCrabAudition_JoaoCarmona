using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class EnemyManager : LeanGameObjectPool
{
    [Header("Custom Fields")]
    [Space(50)]
    [SerializeField] private float _positionSpawnY;
    [SerializeField] private float _maxXSpawnRandom;
    private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemyManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(EnemyManager).Name);
                    _instance = singletonObject.AddComponent<EnemyManager>();
                }
            }
            //DontDestroyOnLoad(_instance.gameObject);

            return _instance;
        }
    }
    private void Start() {
        StartCoroutine(Loop());
    }
    private IEnumerator Loop(){
        yield return new WaitForSeconds(5f);
        Spawn(new Vector3(Random.Range(-_maxXSpawnRandom, _maxXSpawnRandom), _positionSpawnY, 0), Quaternion.identity);
        StartCoroutine(Loop());
    }
    public void Remove(GameObject obj){
        Despawn(obj);
    }
}

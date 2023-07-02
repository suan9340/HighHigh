using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region SingleTon

    private static EnemyManager _instance = null;
    public static EnemyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemyManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("EnemyManager").AddComponent<EnemyManager>();
                }
            }
            return _instance;
        }
    }


    #endregion

    [Header("[[ Current Enemy List ]]")]
    public List<GameObject> enemy = new List<GameObject>();
    public GameObject enemysMomObj = null;
    private GameObject nearObj = null;


    [Space(30)]
    [Header("[[ PlayerObject ]]")]
    public GameObject playerObj = null;


    private float nearObjIndex = 100f;

    private void Start()
    {
        CheckingNull();
        SetListEnemy();
    }

    private void CheckingNull()
    {
        if (enemysMomObj == null)
        {
            Debug.LogError("EnemysMomObj is NULL!!!!!!");
        }

        if (playerObj == null)
        {
            Debug.LogError("PlayerObj is NULL!!!");
        }
    }


    private void SetListEnemy()
    {
        var _childIdx = enemysMomObj.transform.childCount;

        //Debug.Log(_childIdx);

        for (int i = 0; i < _childIdx; i++)
        {
            enemy.Add(enemysMomObj.transform.GetChild(i).gameObject);
        }
    }

    public GameObject NearEnemyCheck()
    {
        CheckingPlayerYEnemysDistance();

        return nearObj;
    }

    private void CheckingPlayerYEnemysDistance()
    {
        foreach (var _enm in enemy)
        {
            var _distance = Vector3.Distance(playerObj.transform.position, _enm.transform.position);

            if (_distance < nearObjIndex)
            {
                nearObjIndex = _distance;
                nearObj = _enm.gameObject;
            }
        }
    }
}   

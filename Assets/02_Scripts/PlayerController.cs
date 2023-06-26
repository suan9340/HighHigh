using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour
{
    private RaycastHit hitInfo;
    private Ray ray;

    private Vector3 rayOrigin = Vector3.zero;
    private Vector3 rayDir = Vector3.zero;

    [Header("Player States")]
    public DefineManager.PlayerState playerState = DefineManager.PlayerState.Idle;


    [Space(20)]
    [Header("RayCastStartPosition")]
    public Transform rayStartTrn = null;
    public float maxRay = 100f;

    [Header("Particles")]
    public GameObject lineEndParticle = null;


    [Space(30)]
    [Header("PlayerMoveSpeed")]
    public float playerMoveSpeed = 3f;

    [Header("EnemyMoveSpeed")]
    public float enemyMoveSpeed = 2f;

    private bool isShootLine = false;
    private Vector3 playerEndVec = Vector3.zero;

    // Cashing
    private Camera mainCam = null;
    private LineRenderer myLineRen = null;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCam = Camera.main;
        myLineRen = GetComponent<LineRenderer>();
        lineEndParticle.SetActive(true);

        DontShootLineRenderer();
    }

    private void Update()
    {
        if (playerState == DefineManager.PlayerState.Moving)
        {
            return;
        }

        InputKey();
    }

    private void InputKey()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isShootLine)
            {
                lineEndParticle.SetActive(true);
                myLineRen.positionCount = 2;
            }

            isShootLine = true;

            CheckHit();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShootLine = false;

            CheckHit();

            DontShootLineRenderer();
        }
    }

    private void DontShootLineRenderer()
    {
        myLineRen.positionCount = 0;
        lineEndParticle.SetActive(false);
    }

    private void CheckHit()
    {
        rayOrigin = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        rayDir = mainCam.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDir, out hitInfo, maxRay))
        {
            if (!isShootLine)
            {
                if (hitInfo.collider.CompareTag("Wall"))
                {
                    Debug.Log("Wall!!");
                    playerEndVec = hitInfo.point;
                    MovePlayer(playerEndVec);
                }
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy!!");
                    EnemyMove(hitInfo.transform.gameObject);
                }
            }
        }

        lineEndParticle.transform.position = hitInfo.point;

        myLineRen.SetPosition(0, rayStartTrn.position);
        myLineRen.SetPosition(1, hitInfo.point);
    }

    #region PlayerMove
    private void MovePlayer(Vector3 _endPos)
    {
        StartCoroutine(ObjectMoveToObject(gameObject, transform.position, _endPos, playerMoveSpeed));
    }
    #endregion


    #region EnemyCatch
    private void EnemyMove(GameObject _enemy)
    {
        StartCoroutine(ObjectMoveToObject(_enemy, _enemy.transform.position, rayStartTrn.transform.position, enemyMoveSpeed));
    }

    #endregion


    private IEnumerator ObjectMoveToObject(GameObject _obj, Vector3 _startPos, Vector3 _endPos, float _time)
    {
        playerState = DefineManager.PlayerState.Moving;
        var _curTime = 0f;
        while (_curTime < _time)
        {
            _curTime += Time.deltaTime;
            _obj.transform.position = Vector3.Slerp(_startPos, _endPos, _curTime / _time);

            yield return null;
        }

        _obj.transform.position = _endPos;
        playerState = DefineManager.PlayerState.Idle;

        yield break;
    }

}

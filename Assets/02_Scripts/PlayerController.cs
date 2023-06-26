using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour
{
    // RayCast
    private RaycastHit hitInfo;
    private Ray ray;

    private Vector3 rayOrigin = Vector3.zero;
    private Vector3 rayDir = Vector3.zero;

    private bool isShootLine = false;
    private Vector3 playerEndVec = Vector3.zero;


    // CameraMoving
    private float rotationY = 0f;
    private float rotationX = 0f;



    // Cashing
    private Camera mainCam = null;
    private LineRenderer myLineRen = null;


    [Header("[[ Player States ]]")]
    public DefineManager.PlayerState playerState = DefineManager.PlayerState.Idle;


    [Space(30)]
    [Header("[[ Rays Info ]]")]
    [Header("RayCastStartPosition")]
    public Transform rayStartTrn = null;
    public float maxRay = 100f;

    [Header("Particles")]
    public GameObject lineEndParticle = null;


    [Space(30)]
    [Header("[[ Speeds ]]")]
    public float playerMoveSpeed = 3f;
    public float enemyMoveSpeed = 2f;


    [Space(30)]
    [Header("Player Settings")]
    public float sensitivity = 5f;


    public GameObject aaaaaaa = null;

    private void Start()
    {
        FirstSetting();
    }

    private void Update()
    {
        if (playerState == DefineManager.PlayerState.Moving)
        {
            return;
        }

        CameraMoveRotation();
        InputKey();
    }

    #region PlayerCameraRotation
    private void CameraMoveRotation()
    {
        float _mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        float _mouseX = Input.GetAxis("Mouse X") * sensitivity;

        rotationY -= _mouseY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);

        rotationX = mainCam.transform.eulerAngles.y + _mouseX;
        mainCam.transform.eulerAngles = new Vector3(rotationY, rotationX, 0);
    }
    #endregion

    private void FirstSetting()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCam = Camera.main;
        myLineRen = GetComponent<LineRenderer>();
        lineEndParticle.SetActive(true);

        DontShootLineRenderer();
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
        aaaaaaa = EnemyManager.Instance.NearEnemyCheck();
        mainCam.transform.LookAt(aaaaaaa.transform.position);

        StartCoroutine(ObjectMoveToObjectSLerp(gameObject, transform.position, _endPos, playerMoveSpeed));
    }
    #endregion


    #region EnemyCatch
    private void EnemyMove(GameObject _enemy)
    {
        StartCoroutine(ObjectMoveToObjectLerp(_enemy, _enemy.transform.position, rayStartTrn.transform.position, enemyMoveSpeed));
    }

    #endregion

    #region Lerp Y Slerp Movement Function
    private IEnumerator ObjectMoveToObjectLerp(GameObject _obj, Vector3 _startPos, Vector3 _endPos, float _time)
    {
        playerState = DefineManager.PlayerState.Moving;
        var _curTime = 0f;
        while (_curTime < _time)
        {
            _curTime += Time.deltaTime;
            _obj.transform.position = Vector3.Lerp(_startPos, _endPos, _curTime / _time);

            yield return null;
        }

        _obj.transform.position = _endPos;
        playerState = DefineManager.PlayerState.Idle;

        yield break;
    }

    private IEnumerator ObjectMoveToObjectSLerp(GameObject _obj, Vector3 _startPos, Vector3 _endPos, float _time)
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
    #endregion
}

using System.Collections;
using System.Collections.Generic;
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



    // PlayerMoving
    private Transform startPos;
    private Transform endPos;


    // Cashing
    private Camera mainCam = null;
    private LineRenderer myLineRen = null;
    private Rigidbody myrigid = null;

    // parabolic movement
    private Vector3 sunrise; //포물선 시작위치
    public Vector3 sunset; //포물선 종료위치
    private float startTime;


    [Header("[[ Player States ]]")]
    public DefineManager.PlayerState playerState = DefineManager.PlayerState.Idle;


    [Space(30)]
    [Header("[[ Rays Info ]]")]
    [Header("RayCastStartPosition")]
    public Transform rayStartTrn = null;
    public float maxRay = 100f;

    [Header("Particles")]
    public GameObject CircleParticle = null;
    public GameObject XParticle = null;


    [Space(30)]
    [Header("[[ Speeds ]]")]
    public float playerMoveSpeed = 3f;
    public float enemyMoveSpeed = 2f;


    [Space(30)]
    [Header("Player Settings")]
    public float sensitivity = 5f;


    [Space(30)]
    [Header("Parabolic Movement")]
    public float journeyTime = 1.0F;
    public float reduceHeight = 1f;

    public GameObject nearEnemy = null;

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
        myrigid = GetComponent<Rigidbody>();

        DontShootLineRenderer();
    }

    private void InputKey()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isShootLine)
            {
                CircleParticle.SetActive(true);
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
        CircleParticle.SetActive(false);
        XParticle.SetActive(false);
    }

    private void CheckHit()
    {
        rayOrigin = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        rayDir = mainCam.transform.forward;

        Debug.DrawRay(rayOrigin, rayDir * 1000f, Color.red);
        if (Physics.Raycast(rayOrigin, rayDir, out hitInfo, maxRay))
        {
            if (!isShootLine)
            {
                CheckHitObject();
            }
        }

        PlayerRayEndParticle();

        myLineRen.SetPosition(0, rayStartTrn.position);
        myLineRen.SetPosition(1, hitInfo.point);
    }

    private void CheckHitObject()
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
        if (hitInfo.collider.CompareTag("FakeWall"))
        {
            Debug.Log("Fake Wall!!");
        }
    }

    private void PlayerRayEndParticle()
    {
        if (hitInfo.collider.name == "FakeWall")
        {
            if (CircleParticle.activeSelf)
            {
                CircleParticle.SetActive(false);
                XParticle.SetActive(true);
            }

            XParticle.transform.position = hitInfo.point;
        }
        else
        {
            if (XParticle.activeSelf)
            {
                XParticle.SetActive(false);
                CircleParticle.SetActive(true);
            }

            CircleParticle.transform.position = hitInfo.point;
        }
    }

    #region PlayerMove
    private void MovePlayer(Vector3 _endPos)
    {
        //StartCoroutine(ObjectMoveToObjectSLerp(gameObject, transform.position, _endPos, playerMoveSpeed));
        sunrise = transform.position;
        sunset = _endPos;
        StartCoroutine(MovePlayerPlablor());
    }
    #endregion


    #region EnemyCatch
    private void EnemyMove(GameObject _enemy)
    {
        StartCoroutine(ObjectMoveToObjectLerp(_enemy, _enemy.transform.position, rayStartTrn.transform.position, enemyMoveSpeed));
    }

    private IEnumerator ObjectMoveToObjectLerp(GameObject _obj, Vector3 _startPos, Vector3 _endPos, float _time)
    {
        playerState = DefineManager.PlayerState.Moving;

        _obj.transform.LookAt(gameObject.transform);
        var _curTime = 0f;
        while (_curTime < _time)
        {
            _curTime += Time.deltaTime;
            _obj.transform.position = Vector3.Lerp(_startPos, _endPos, _curTime / _time);

            yield return null;
        }

        _obj.transform.position = _endPos;

        _obj.GetComponent<Animator>().SetTrigger("isDie");
        playerState = DefineManager.PlayerState.Idle;

        yield break;
    }

    #endregion

    #region Lerp Y Slerp Movement Function



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
        nearEnemy = EnemyManager.Instance.NearEnemyCheck();


        _curTime = 0f;
        while (_curTime < 1)
        {
            _curTime += Time.deltaTime;
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, nearEnemy.transform.rotation, _curTime / _time);
            yield return null;
        }

        //mainCam.transform.LookAt(nearEnemy.transform.position);


        playerState = DefineManager.PlayerState.Idle;
        yield return null;
    }

    #endregion

    #region Player ParaBolic MoveMent
    private IEnumerator MovePlayerPlablor()
    {
        playerState = DefineManager.PlayerState.Moving;

        var _curTime = 0f;


        while (_curTime < journeyTime)
        {
            _curTime += Time.deltaTime;

            if (transform.position == sunset)
            {
                yield break;
            }

            Vector3 center = (sunrise + sunset) * 0.5F; //Center 값만큼 위로 올라간다.
            center -= new Vector3(0, 1f * reduceHeight, 0); //y값을 높이면 높이가 낮아진다.

            Vector3 riseRelCenter = sunrise - center;
            Vector3 setRelCenter = sunset - center;

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, _curTime / journeyTime);
            transform.position += center;
            yield return null;
        }

        playerState = DefineManager.PlayerState.Idle;
    }
    #endregion

}

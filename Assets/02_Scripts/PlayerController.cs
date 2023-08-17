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
    private Vector3 sunset; //포물선 종료위치
    private float startTime;


    [Header("[[ Player States ]]")]
    public DefineManager.PlayerState playerState = DefineManager.PlayerState.Idle;

    [Header("[[ Player Settings ]]")]
    public float sensitivity = 5f;


    [Space(30)]
    [Header("[[ Rays Info ]]")]
    [Header("RayCastStartPosition")]
    public Transform rayStartTrn = null;
    public float maxRay = 100f;

    [Header("[[ Particles ]]")]
    public GameObject CircleParticle = null;
    public GameObject XParticle = null;


    [Space(30)]
    [Header("[[ Speeds ]]")]
    public float playerMoveSpeed = 3f;
    public float enemyMoveSpeed = 2f;



    [Space(30)]
    [Header("[[ Parabolic Movement ]]")]
    public float playerMoveTime = 1.0F;
    public float reduceHeight = 1f;
    public GameObject nearEnemy = null;
    public string colString = "";


    [Space(30)]
    [Header("[[ Player Shoot Line ]]")]
    public LineRenderer shootLine = null;

    private void Start()
    {
        FirstSetting();
    }

    private void Update()
    {
        if (playerState == DefineManager.PlayerState.Moving || GameManager.Instance.gameState == DefineManager.GameState.Menu)
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
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        mainCam = Camera.main;

        myLineRen = GetComponent<LineRenderer>();
        myrigid = GetComponent<Rigidbody>();

        myLineRen.positionCount = 2;

        DontShootLineRenderer();
    }

    private void InputKey()
    {
        if (Input.GetMouseButton(0))
        {
            CheckHit();
        }
        if (Input.GetMouseButtonUp(0))
        {
            MouseUPCheckY();
            DontShootLineRenderer();
        }
    }

    private void DontShootLineRenderer()
    {
        ResetLineRenderers(myLineRen);

        CircleParticle.SetActive(false);
        XParticle.SetActive(false);

        colString = "";
    }

    #region PlayerRayCheck


    private void CheckHit()
    {
        rayOrigin = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        rayDir = mainCam.transform.forward;

        Debug.DrawRay(rayOrigin, rayDir * 100f, Color.red);
        if (Physics.Raycast(rayOrigin, rayDir, out hitInfo, maxRay))
        {
            CheckHitObject();
        }

        PlayerRayEndParticle();

        myLineRen.SetPosition(0, rayStartTrn.position);
        myLineRen.SetPosition(1, hitInfo.point);
    }

    private void CheckHitObject()
    {
        if (hitInfo.collider.CompareTag("Wall"))
        {
            if (colString == "Wall")
                return;

            colString = "Wall";

        }
        if (hitInfo.collider.CompareTag("Enemy"))
        {
            if (colString == "Enemy")
                return;

            colString = "Enemy";
        }
        if (hitInfo.collider.CompareTag("FakeWall"))
        {
            if (colString == "FakeWall")
                return;

            colString = "FakeWall";
        }
    }

    private void MouseUPCheckY()
    {
        switch (colString)
        {
            case "Wall":
                playerEndVec = hitInfo.point;
                MovePlayer(playerEndVec);
                break;

            case "Enemy":
                EnemyMove(hitInfo.transform.gameObject);
                break;

            case "FakeWall":

                break;

            default:
                Debug.Log("QUE!???!!?");
                break;
        }
    }

    private void PlayerRayEndParticle()
    {
        if (hitInfo.collider == null || colString == "")
        {
            return;
        }

        switch (colString)
        {
            case "Wall":
            case "Enemy":
                CircleParticle.SetActive(true);
                XParticle.SetActive(false);

                CircleParticle.transform.position = hitInfo.point;
                break;

            case "FakeWall":
                CircleParticle.SetActive(false);
                XParticle.SetActive(true);

                XParticle.transform.position = hitInfo.point;
                break;

            default:
                Debug.Log("QUE!???!!?");
                break;
        }
    }

    #endregion

    #region PlayerMove
    private void MovePlayer(Vector3 _endPos)
    {
        sunrise = transform.position;
        sunset = _endPos;
        StartCoroutine(MovePlayerPlablor());
    }

    private IEnumerator MovePlayerPlablor()
    {
        playerState = DefineManager.PlayerState.Moving;

        shootLine.SetPosition(0, hitInfo.point);
        var _curTime = 0f;

        while (_curTime < playerMoveTime)
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

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, _curTime / playerMoveTime);
            transform.position += center;
            shootLine.SetPosition(1, rayStartTrn.transform.position);
            yield return null;
        }


        yield return PlayerRotationToEnemy();
    }

    private IEnumerator PlayerRotationToEnemy()
    {
        var _curTime = 0f;

        nearEnemy = EnemyManager.Instance.NearEnemyCheck();

        while (_curTime < 1)
        {
            _curTime += Time.deltaTime;
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, nearEnemy.transform.rotation, _curTime / 1);
            yield return null;
        }
        playerState = DefineManager.PlayerState.Idle;

        ResetLineRenderers(shootLine);
        yield return null;
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

    #region Etc
    private void ResetLineRenderers(LineRenderer _rend)
    {
        _rend.SetPosition(0, Vector3.zero);
        _rend.SetPosition(1, Vector3.zero);
    }
    #endregion
}

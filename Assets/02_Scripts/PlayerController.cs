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


    [Header("RayCastStartPosition")]
    public Transform rayStartTrn = null;
    public float maxRay = 100f;

    [Header("Particles")]
    public GameObject lineEndParticle = null;

    [Header("PlayerMoveSpeed")]
    public float playerMoveSpeed = 3f;

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
            CheckHit();
            isShootLine = false;
            MovePlayer(playerEndVec);
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
            if (hitInfo.collider.CompareTag("Wall"))
            {

            }

            playerEndVec = hitInfo.point;
        }

        lineEndParticle.transform.position = hitInfo.point;

        myLineRen.SetPosition(0, rayStartTrn.position);
        myLineRen.SetPosition(1, hitInfo.point);
    }

    private void MovePlayer(Vector3 _endPos)
    {
        //transform.position = _endPos;
        StartCoroutine(PlayerSlerpMove(_endPos));
    }

    private IEnumerator PlayerSlerpMove(Vector3 _targetPos)
    {
        var _curTime = 0f;
        while (_curTime < playerMoveSpeed)
        {
            _curTime += Time.deltaTime;
            transform.position = Vector3.Slerp(transform.position, _targetPos, _curTime / playerMoveSpeed);

            yield return null;
        }

        transform.position = _targetPos;

        yield return null;
    }
}

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

    private bool isShootLine = false;
    private Vector3 playerEndVec = Vector3.zero;

    // Cashing
    private Camera mainCam = null;
    private LineRenderer myLineRen = null;

    public GameObject wall;

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
                myLineRen.positionCount = 2;
            }

            isShootLine = true;

            CheckHit();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isShootLine = false;
            MovePlayer(playerEndVec);
            DontShootLineRenderer();
        }
    }

    private void DontShootLineRenderer()
    {
        myLineRen.positionCount = 0;
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
        Debug.Log(_endPos);

        //Instantiate(wall, _endPos, Quaternion.identity);
        transform.position = _endPos;

    }



}

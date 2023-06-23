using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour
{
    private RaycastHit hitInfo;
    private Ray ray;


    [Header("RayCastStartPosition")]
    public Transform rayStartTrn = null;
    public float maxRay = 100f;

    [Header("Particles")]
    public GameObject lineEndParticle = null;

    private Camera mainCam = null;
    private LineRenderer myLineRen = null;

    private bool isShootLine = false;
    private Vector3 playerEndVec = Vector3.zero;

    private void Start()
    {
        mainCam = Camera.main;
        myLineRen = GetComponent<LineRenderer>();

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
            CheckHit();
            ShootLineRenderer();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isShootLine = false;
            MovePlayer(playerEndVec);
            DontShootLineRenderer();
        }
    }

    private void ShootLineRenderer()
    {
        myLineRen.positionCount = 2;
        myLineRen.SetPosition(0, rayStartTrn.position);
        myLineRen.SetPosition(1, mainCam.transform.forward * 100);
    }

    private void DontShootLineRenderer()
    {
        myLineRen.positionCount = 0;
    }

    private void CheckHit()
    {
        if (Physics.Raycast(rayStartTrn.position, mainCam.transform.forward, out hitInfo, maxRay))
        {
            lineEndParticle.transform.position = hitInfo.point;

            if (hitInfo.collider.CompareTag("Wall"))
            {
                if (isShootLine)
                {
                    return;
                }

                isShootLine = true;
                playerEndVec = hitInfo.point;

                lineEndParticle.SetActive(true);

            }
        }

    }

    private void MovePlayer(Vector3 _endPos)
    {
        Debug.Log(_endPos);

        transform.position = _endPos;
    }



}

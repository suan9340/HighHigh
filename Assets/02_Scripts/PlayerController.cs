using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RaycastHit hitInfo;
    private Ray ray;

    private Camera mainCam = null;

    [Header("RayCastStartPosition")]
    public Transform rayStartTrn = null;
    public float maxRay = 100f;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        Debug.DrawRay(rayStartTrn.position, mainCam.transform.forward * 100, Color.red);
        CheckHit();
    }

    private void CheckHit()
    {
        if (Physics.Raycast(rayStartTrn.position, mainCam.transform.forward, out hitInfo, maxRay))
        {
            Debug.Log(hitInfo.transform.name);
        }

    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RaycastHit rayHit;
    private Ray ray;

    private Camera mainCam = null;

    [Header("RayCastStartPosition")]
    public Transform rayStartTrn = null;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {

    }

}

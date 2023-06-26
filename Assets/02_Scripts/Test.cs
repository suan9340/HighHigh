using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Camera playerCam;

    public float distance = 100f;

    public GameObject lineEndParticle = null;

    private RaycastHit hitInfo;
    void Start()
    {
        playerCam = Camera.main;
        lineEndParticle.SetActive(true);
    }

    void Update()
    {
        Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        Vector3 rayDir = playerCam.transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(rayOrigin, rayDir, out hitInfo, distance))
            {
                Debug.Log("¹º°¡ ±¤¼±¿¡ °É·È´Ù!");
            }
        }

        Debug.DrawRay(rayOrigin, rayDir * 1000, Color.red);
        lineEndParticle.transform.position = hitInfo.point;
    }

}



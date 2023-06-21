using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [Header("Players Sensitivity")]
    public float sensitivity = 5f;

    private float rotationY = 0f;
    private float rotationX = 0f;

    private Camera myCam = null;

    private void Start()
    {
        myCam = Camera.main;
    }

    void Update()
    {
        //CameraUpDown();
        //CameraLeftRight();

        float _mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        float _mouseX = Input.GetAxis("Mouse X") * sensitivity;

        rotationY -= _mouseY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);


        rotationX = transform.eulerAngles.y + _mouseX;
        transform.eulerAngles = new Vector3(rotationY, rotationX, 0);
    }

    private void CameraUpDown()
    {
        float _mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationY -= _mouseY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);
        transform.localEulerAngles = new Vector3(rotationY, 0, 0);
    }

    private void CameraLeftRight()
    {
        float _mouseX = Input.GetAxis("Mouse X") * sensitivity;
        Vector3 _rotateX = new Vector3(0, _mouseX, 0);



        //transform.localEulerAngles += new Vector3(0f, _mouseX, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [Header("Players Sensitivity")]
    public float sensitivity = 5f;

    private float rotationY = 0f;
    private float rotationX = 0f;

    void Update()
    {
        float _mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        float _mouseX = Input.GetAxis("Mouse X") * sensitivity;

        rotationY -= _mouseY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);


        rotationX = transform.eulerAngles.y + _mouseX;
        transform.eulerAngles = new Vector3(rotationY, rotationX, 0);
    }
}

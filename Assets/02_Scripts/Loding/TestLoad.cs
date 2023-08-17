using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoad : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadingSceneManager.Instance.LoadScene("Main");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    /// <summary>
    /// Click Start Btn 
    /// </summary>
    public void OnClickStart()
    {
        Debug.Log("Start");
        SceneManager.LoadScene("Main");
    }
}

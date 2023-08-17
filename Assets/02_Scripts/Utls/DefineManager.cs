using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineManager : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Moving,
        EnemyHit,
    }

    public enum GameState
    {
        Setting,
        Menu,
        Playing,
        Quit,
    }
}

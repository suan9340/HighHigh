using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefineManager
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
        Playing,
        Quit,
    }
}

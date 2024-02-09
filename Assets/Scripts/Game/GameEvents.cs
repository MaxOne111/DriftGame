using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static Action _On_Level_Started;
    public static Action _On_Level_Ended;

    public static void OnLevelStarted()
    {
        _On_Level_Started?.Invoke();
    }

    public static void OnLevelEnded()
    {
        _On_Level_Ended?.Invoke();
    }
}

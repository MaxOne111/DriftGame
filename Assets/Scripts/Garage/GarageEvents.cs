using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GarageEvents
{
    public static Action _On_Purchased;

    public static void OnPurchased()
    {
        _On_Purchased?.Invoke();
    }
}

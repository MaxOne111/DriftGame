using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhotonEvents
{
    public static Action _On_Joined_Room;

    public static void OnJoinedRoom()
    {
        _On_Joined_Room?.Invoke();
    }

}

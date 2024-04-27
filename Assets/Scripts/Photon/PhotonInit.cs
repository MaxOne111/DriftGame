using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _Regoin;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(_Regoin);

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"Connect to: {PhotonNetwork.CloudRegion}");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnect from: {PhotonNetwork.CloudRegion}");
    }

}
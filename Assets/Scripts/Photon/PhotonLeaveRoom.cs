using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLeaveRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _Leave_Room;
    [SerializeField] private Transform _Button_Place;

    private void Start()
    {
        if (SceneMediator.IsPhoton)
        {
            InstantiateButton();
        }
    }

    private void InstantiateButton()
    {
        Button _leave_Room = Instantiate(_Leave_Room, _Button_Place);
        _leave_Room.onClick.AddListener(LeaveRoom);
    }
    
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Garage");
        
        SceneMediator.IsPhoton = false;
        SceneMediator.IsHost = false;
    }
    
    
}

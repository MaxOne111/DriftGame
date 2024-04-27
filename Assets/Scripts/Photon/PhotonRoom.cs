using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _Input;
    [SerializeField] private RoomListItem _List_Item;
    [SerializeField] private Transform _Content;
    [SerializeField] private LevelsMenu _Levels_Menu;

    private List<RoomInfo> _Rooms;
    
    public void OpenLevelsMenu()
    {
        if (_Input.text.Length <= 0)
            return;
        
        _Levels_Menu.gameObject.SetActive(true);
        _Levels_Menu.PhotonTypeLoad(this);
    }
    
    public void CreateRoom()
    {
        if(!PhotonNetwork.IsConnected)
            return;

        if (_Input.text.Length <=0)
            return;
        
        
        RoomOptions _options = new RoomOptions();
        _options.MaxPlayers = 2;
        
        PhotonNetwork.CreateRoom(_Input.text, _options, TypedLobby.Default);
    }
    
    public override void OnCreatedRoom()
    {

        Debug.Log($"Room has benn created: {PhotonNetwork.CurrentRoom.Name}");
        
        SceneMediator.IsHost = true;
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation is failed");
    }

    public override void OnRoomListUpdate(List<RoomInfo> _room_List)
    {
        _Rooms = new List<RoomInfo>();

        foreach(RoomInfo _info in _room_List)
        {
            for (int i = 0; i < _Rooms.Count; i++)
            {
                if (_Rooms[i].masterClientId == _info.masterClientId)
                    return;
            }

            if (_info.PlayerCount == 0)
                return;
            
            RoomListItem _item = Instantiate(_List_Item, _Content);
            
            if (_item != null)
            {
                _Rooms.Add(_info);
                _item.SetInfo(_info);
            }
        }
        
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
        SceneMediator.IsPhoton = true;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _Room_Name;
    [SerializeField] private TextMeshProUGUI _Player_Count;

    private Button _Button;

    private void Awake()
    {
        _Button = GetComponent<Button>();
        _Button.onClick.AddListener(JoinToListRoom);
    }

    public void SetInfo(RoomInfo _info)
    {
        _Room_Name.text = _info.Name;
        _Player_Count.text = $"{_info.PlayerCount}/{_info.MaxPlayers}";
    }

    private void JoinToListRoom()
    {
        PhotonNetwork.JoinRoom(_Room_Name.text);
    }
}

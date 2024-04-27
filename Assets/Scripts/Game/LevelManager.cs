using System;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Level[] _Levels;
    [SerializeField] private CameraInitialize _Camera;
    private PhotonView _Photon_View;

    private Car _Car;
    
    private Transform _Start_Position_Host;
    private Transform _Start_Position_Client;

    private Transform _Start_Position;
    
    private void Awake() => _Photon_View = GetComponent<PhotonView>();

    private void Start()
    {
        if (SceneMediator.IsHost)
            _Photon_View.RPC("LevelInit", RpcTarget.AllBuffered, SceneMediator.LevelIndex);
        
        _Start_Position = _Levels[SceneMediator.LevelIndex].StartPositionClient;
        
        if (SceneMediator.IsHost)
            _Start_Position =  _Levels[SceneMediator.LevelIndex].StartPositionHost;
        

        if (!SceneMediator.IsPhoton)
        {
            LevelInit(SceneMediator.LevelIndex);
            _Start_Position = _Start_Position_Client;
        }
        
        CarInstance();

        _Camera.Init(_Car.transform);
        
    }

   [PunRPC]
    private void LevelInit(int _level_Index)
    {
        if (!_Levels[_level_Index])
            return;
        
        _Levels[_level_Index].Init();

        _Start_Position_Host = _Levels[_level_Index].StartPositionHost;
        _Start_Position_Client = _Levels[_level_Index].StartPositionClient;
        
        
    }

    private void CarInstance()
    {
        if (SceneMediator.IsPhoton)
        {
            _Car = PhotonNetwork.Instantiate(SceneMediator.SelectedCar.name, _Start_Position.position, Quaternion.identity).GetComponent<Car>();
            return;
        }

        _Car = Instantiate(SceneMediator.SelectedCar, _Start_Position.position, Quaternion.identity);
    }
    
    
    public override void OnLeftRoom()
    {
        PhotonNetwork.Destroy(_Car.gameObject);
    }

 
}
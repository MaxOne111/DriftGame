using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LevelPassing : MonoBehaviour
{
    [SerializeField] private GameUI _Game_UI;
    [Range(0,59)]
    [SerializeField] private float _Time_For_Passing_Min;
    [Range(0,59)]
    [SerializeField] private float _Time_For_Passing_Sec;

    [PunRPC]
    private float _Remaining_Time_Sec = 0;
    [PunRPC]
    private float _Remaining_Time_Min = 0;

    private PhotonView _Photon_View;

    private void Awake() => _Photon_View = GetComponent<PhotonView>();

    private void Start() => StartCoroutine(StartLevel());

    private IEnumerator StartLevel()
    {
        GameEvents.OnLevelStarted();

        _Remaining_Time_Sec = _Time_For_Passing_Sec;
        _Remaining_Time_Min = _Time_For_Passing_Min;

        while (_Remaining_Time_Sec > 0 || _Remaining_Time_Min > 0)
        {

            if (_Remaining_Time_Sec <= 0)
            {
                _Remaining_Time_Sec = 60;
                _Remaining_Time_Min -= 1;
            }

            yield return new WaitForSeconds(1);
            _Remaining_Time_Sec -= 1;

             if (SceneMediator.IsHost)
                 _Photon_View.RPC("SynchronizeTime", RpcTarget.AllBuffered, _Remaining_Time_Sec, _Remaining_Time_Min);

             if (!SceneMediator.IsPhoton)
                 _Game_UI.RemainingTime(_Remaining_Time_Sec, _Remaining_Time_Min);
             
             
            yield return null;
        }

        if (SceneMediator.IsHost)
            _Photon_View.RPC("LevelFinish", RpcTarget.AllBuffered);
        

        if (!SceneMediator.IsPhoton)
            LevelFinish();
    }

    [PunRPC]
    private void LevelFinish()
    {
        GameEvents.OnLevelEnded();
    }

    [PunRPC]
    private void SynchronizeTime(float _sec, float _min)
    {
        _Game_UI.RemainingTime(_sec, _min);
    }
    
}

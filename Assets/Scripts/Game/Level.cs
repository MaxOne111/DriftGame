using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int _Level_Index;
    [SerializeField] private Transform _Start_Point_Host;
    [SerializeField] private Transform _Start_Point_Client;
    
    public int LevelIndex{get=>_Level_Index;}
    public Transform StartPositionHost{get=>_Start_Point_Host;}
    public Transform StartPositionClient{get=>_Start_Point_Client;}

    public void Init()
    {
        gameObject.SetActive(true);
    }

}
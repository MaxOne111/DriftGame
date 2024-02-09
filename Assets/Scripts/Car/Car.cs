using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Car : MonoBehaviour
{
    [SerializeField] private CarData _Data;
    [SerializeField] private CustomizeData _Customize;
    [SerializeField] private Renderer _Body;
    
    private CarMovement _Movement;

    private void Awake()
    {
        _Movement = GetComponent<CarMovement>();
        _Data.Init(_Movement.MaxSpeed, _Movement.DriftForce,this);
        
        _Customize.Init(this);
    }

    public CarData Data{get=>_Data;}
    public CustomizeData Customize{get=>_Customize;}
    public Renderer Body{get=>_Body;}
}

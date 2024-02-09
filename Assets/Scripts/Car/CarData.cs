using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Car data")]
public class CarData : ScriptableObject
{
    [SerializeField] private string _Name;
    [SerializeField] private int _Price;
    [SerializeField] private Car _Prefab;

    public Car CarReference { get; set; }
    public string Name{get=>_Name;}
    public int Price{get=>_Price;}
    
    public float MaxSpeed { get; private set; }
    public float DriftForce { get; private set; }
    public Car Prefab { get=>_Prefab; }

    public void Init(float _max_Speed, float _drift_Force, Car _car)
    {
        MaxSpeed = _max_Speed;
        DriftForce = _drift_Force;
        CarReference = _car;
    }

}

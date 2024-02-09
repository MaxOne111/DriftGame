using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Customize data")]
public class CustomizeData : ScriptableObject
{
    [SerializeField] private Material[] _Materials;

    private Renderer _Body;
    private Renderer _Prefab;
    public Material[] Materials{get=>_Materials;}
    

    public void Init(Car _car)
    {
        _Body = _car.Body;
        _Prefab = _car.Data.Prefab.Body;
    }

    public void SetColor(int _color_Index)
    {
        _Body.sharedMaterial = _Materials[_color_Index];
        _Prefab.sharedMaterial = _Materials[_color_Index];
    }
}

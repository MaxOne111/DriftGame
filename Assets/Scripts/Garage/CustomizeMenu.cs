using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeMenu : MonoBehaviour
{
    [SerializeField] private Button _Next_Material;
    [SerializeField] private Button _Prev_Material;

    private int _Material_Index = 0;
    
    private Car _Current_Car;

    private void Awake()
    {
        _Next_Material.onClick.AddListener(delegate { SelectColor(1); });
        _Prev_Material.onClick.AddListener(delegate { SelectColor(-1); });
    }
    
    public void SelectCar(Car _car)
    {
        _Current_Car = _car;
    }
    
    private void SelectColor(int _next_Value)
    {
        Material[] _materials = _Current_Car.Customize.Materials;

        if (_materials.Length == 1)
            _Material_Index = 0;

        _Material_Index += _next_Value;
        if (_Material_Index >= _materials.Length)
        {
            _Material_Index = 0;
        }
        else if (_Material_Index < 0)
        {
            _Material_Index = _materials.Length - 1;
        }
        
        _Current_Car.Customize.SetColor(_Material_Index);
        _Current_Car.Data.Prefab.Customize.SetColor(_Material_Index);
    }
    
}

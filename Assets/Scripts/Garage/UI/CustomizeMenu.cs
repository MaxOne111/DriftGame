using System;
using UnityEngine;

public class CustomizeMenu : MonoBehaviour
{
    [SerializeField] private CustomizePartPanel[] _Part_Types;
    
    private void OnEnable() => CheckParts();
    
    private void CheckParts()
    {
        for (int i = 0; i < _Part_Types.Length; i++)
            _Part_Types[i].CheckParts();
    }
}
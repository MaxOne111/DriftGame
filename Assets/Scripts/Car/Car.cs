using System;
using Photon.Pun;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private CarCharacteristics _Characteristics;
    [SerializeField] private CarShopData _Shop_Data;
    [SerializeField] private CarCustomizeData _CustomizeData;
    [SerializeField] private CarParts _Parts;

    private CarMovement _Movement;

    public CarShopData ShopData => _Shop_Data;

    public CarCharacteristics Characteristics => _Characteristics;

    public CarCustomizeData CustomizeData => _CustomizeData;
    public CarParts Parts => _Parts;
    
    private void Awake()
    {
        _Movement = GetComponent<CarMovement>();
        
        _Parts.Init(SceneMediator.PlayerData._Car_Model.SavedParts);
        
        StartCustomize();
        
        _Movement.WheelsInit();
    }
    
    private void StartCustomize() => _Parts.StartCustomize();
}
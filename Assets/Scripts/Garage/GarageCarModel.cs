using System;
using UnityEngine;

public class GarageCarModel : MonoBehaviour
{
    [SerializeField] private CarShopData _Shop_Data;
    [SerializeField] private CarCharacteristics _Characteristics;
    [SerializeField] private CarCustomizeData _Customize_Data;
    [SerializeField] private CarParts _Car_Parts;

    public CarShopData ShopData => _Shop_Data;
    public CarCharacteristics Characteristics => _Characteristics;
    public CarCustomizeData CustomizeData => _Customize_Data;
    public CarParts CarParts => _Car_Parts;
    public PlayerCarParts SavedParts => _Car_Parts._Saved_Parts;

    private void Start() => StartCustomize();

    private void StartCustomize()
    {
        if (SceneMediator.PlayerData._Car_Model != null)
            _Car_Parts.Init(SceneMediator.PlayerData._Car_Model._Car_Parts._Saved_Parts);

        _Car_Parts.StartCustomize();
    }
}
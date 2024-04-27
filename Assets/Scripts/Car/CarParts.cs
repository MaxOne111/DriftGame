using System;
using JetBrains.Annotations;
using UnityEngine;

public class CarParts : MonoBehaviour
{
    [SerializeField] private Renderer _Car_Renderer;

    [SerializeField] private CarWheels _Wheels_Ref;
    [Header("Spoiler")]
    [SerializeField] private GameObject _Spoiler;
    [SerializeField] private Transform _Spoiler_Place;
    [Header("Bumper")]
    [SerializeField]private GameObject _Bumper;
    [SerializeField] private Transform _Bumper_Place;
    [Header("Wheels")]
    [SerializeField] private GameObject[] _Wheels;
    [SerializeField] private Transform[] _Wheel_Places;
    
    private CarCustomize _Customize = new CarCustomize();
    
    public PlayerCarParts _Saved_Parts;

    public void Init(PlayerCarParts _parts) => _Saved_Parts = _parts;

    public void StartCustomize()
    {
        UninstallSpoiler();
        InstallSpoiler(_Saved_Parts._Spoiler);
        
        UninstallBumper();
        InstallBumper(_Saved_Parts._Bumper);

        if (_Saved_Parts._Wheels != null)
        {
            UninstallWheels();
            InstallWheels(_Saved_Parts._Wheels);
        }

        ChangeColor(_Saved_Parts._Color);
    }

    //----------Install parts----------//
    
    public void InstallSpoiler(GameObject _spoiler)
    {
        if (_spoiler == null)
            return;

        _Spoiler = _Customize.InstallOnCar(_spoiler, _Spoiler_Place);
        _Saved_Parts._Spoiler = _spoiler;
    }

    public void InstallBumper(GameObject _bumper)
    {
        if (_bumper == null)
            return;
        
        _Bumper = _Customize.InstallOnCar(_bumper, _Bumper_Place);
        _Saved_Parts._Bumper = _bumper;
    }

    public void InstallWheels(GameObject _wheel)
    {
        if (_wheel == null)
            return;

        for (int i = 0; i < _Wheels.Length; i++)
            _Wheels[i] = _Customize.InstallOnCar(_wheel, _Wheel_Places[i]);


        _Saved_Parts._Wheels = _wheel;
        
        _Wheels_Ref.InitMeshes(_Wheels);
    }

    public void ChangeColor(Material _color)
    {
        if (_color == null)
            return;
        
        _Customize.InstallColor(_Car_Renderer, _color);
        _Saved_Parts._Color = _color;
    }

    //----------Uninstall parts----------//

    public void UninstallSpoiler() => _Customize.UninstallFromCar(_Spoiler);

    public void UninstallBumper() => _Customize.UninstallFromCar(_Bumper);

    public void UninstallWheels()
    {
        for (int i = 0; i < _Wheels.Length; i++)
            _Customize.UninstallFromCar(_Wheels[i]);
    }
    
}
using System;
using UnityEngine;
using UnityEngine.UI;

public class WheelsPartPanel : CustomizePartPanel
{
    public override void CheckParts()
    {
        SelectCar();
        
        if (_Customize_Data.Wheels.Length == 0)
            _Panel_Button.interactable = false;
    }

    protected override void ChangeCarPart(int nextValue)
    {
        GameObject[] _wheels = _Customize_Data.Wheels;
        
        NextElement(_wheels, nextValue);

        _Car_Parts.UninstallWheels();
        _Car_Parts.InstallWheels(_wheels[_Index]);
        
        _Car_Parts._Saved_Parts._Wheels = _wheels[_Index];

        SaveChanges();
    }
}
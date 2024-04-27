using UnityEngine;

public class BumperPartPanel : CustomizePartPanel
{
    public override void CheckParts()
    {
        SelectCar();
        
        if (_Customize_Data.Bumpers.Length == 0)
            _Panel_Button.interactable = false;
    }
    protected override void ChangeCarPart(int nextValue)
    {
        GameObject[] _bumpers = _Customize_Data.Bumpers;
        
        NextElement(_bumpers, nextValue);

        _Car_Parts.UninstallBumper();
        _Car_Parts.InstallBumper(_bumpers[_Index]);
        
        _Car_Parts._Saved_Parts._Bumper = _bumpers[_Index];

        SaveChanges();
    }
}
using UnityEngine;

public class SpoilerPartPanel : CustomizePartPanel
{
    public override void CheckParts()
    {
        SelectCar();
        
        if (_Customize_Data.Spoilers.Length == 0)
            _Panel_Button.interactable = false;
    }
    protected override void ChangeCarPart(int _next_Value)
    {
        GameObject[] _spoilers = _Customize_Data.Spoilers;
        
        NextElement(_spoilers, _next_Value);
        
        _Car_Parts.UninstallSpoiler();
        _Car_Parts.InstallSpoiler(_spoilers[_Index]);
        
        _Car_Parts._Saved_Parts._Spoiler = _spoilers[_Index];

        SaveChanges();
    }
}
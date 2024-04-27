using UnityEngine;

public class ColorPanel : CustomizePartPanel
{
    protected override void Awake()
    {
        _Prev.onClick.AddListener(delegate { ChangeCarColor(-1); });
        _Next.onClick.AddListener(delegate { ChangeCarColor(1); });
    }

    public override void CheckParts()
    {
        SelectCar();
        
        if (_Customize_Data.Spoilers.Length == 0)
            _Panel_Button.interactable = false;
    }

    protected override void ChangeCarColor(int _next_Value)
    {
        Material[] _colors = _Customize_Data.Colors;
        
        NextElement(_colors, _next_Value);
        
        _Car_Parts.ChangeColor(_colors[_Index]);

        _Car_Parts._Saved_Parts._Color = _colors[_Index];
    }
}
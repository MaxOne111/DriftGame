using UnityEngine;
using UnityEngine.UI;

public abstract class CustomizePartPanel : MonoBehaviour
{
    [SerializeField] protected Button _Next;
    [SerializeField] protected Button _Prev;
    [SerializeField] protected Button _Panel_Button;
    
    protected CarParts _Car_Parts;
    protected CarCustomizeData _Customize_Data;

    protected int _Index = 0;

    protected virtual void ChangeCarPart(int _next_Value){}
    protected virtual void ChangeCarColor(int _next_Value){}
    
    public virtual void CheckParts(){}
    
    protected virtual void Awake()
    {
        _Prev.onClick.AddListener(delegate { ChangeCarPart(-1); });
        _Next.onClick.AddListener(delegate { ChangeCarPart(1); });
    }

    protected void SaveChanges() => GameEvents.GameSaving();
    protected void SelectCar()
    {
        _Car_Parts = SceneMediator.SelectedModel.CarParts;
        _Customize_Data = SceneMediator.PlayerData._Car_Model.CustomizeData;
    }
    

    protected void NextElement<T>(T[] _array, int _next_Element)
    {
        if (_array.Length == 1)
        {
            _Index = 0;
            return;
        }

        _Index += _next_Element;
        
        if (_Index < 0)
        {
            _Index = _array.Length - 1;
            return;
        }

        if (_Index >= _array.Length)
            _Index = 0;
    }
    
}
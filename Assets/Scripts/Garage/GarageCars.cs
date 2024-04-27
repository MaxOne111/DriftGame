using System;
using System.Collections.Generic;
using UnityEngine;

public class GarageCars : MonoBehaviour
{
    [SerializeField] private List<GarageCarModel> _Model_Prefabs;

    [SerializeField] private Transform _Models_Position;
    
    [SerializeField] private GarageCarModel _Default_Model;
    
    [SerializeField] private List<GarageCarModel> _Models_On_Scene = new List<GarageCarModel>();
    
    private int _Car_Index = 0;
    
    private PlayerData _Player_Data;

    public event Action<GarageCarModel> _On_Model_Created;
    

    public void Init() => _Player_Data = SceneMediator.PlayerData;

    public void CreateModels()
    {
        for (int i = 0; i < _Model_Prefabs.Count; i++)
        {
            if (_Player_Data._Car_Model == null)
            {
                if (_Model_Prefabs[i].ShopData.Name == _Default_Model.ShopData.Name)
                    continue;
            }
            else
            if (_Model_Prefabs[i].ShopData.Name == _Player_Data._Car_Model.ShopData.Name)
                    continue;
            
            GarageCarModel _model =  Instantiate(_Model_Prefabs[i], _Models_Position);

            _Models_On_Scene.Add(_model);
            
            _model.gameObject.SetActive(false);
        }
        
        if (_Player_Data._Car_Model == null)
        {
            GarageCarModel _default_Model = Instantiate(_Default_Model, _Models_Position);

            _Models_On_Scene.Insert(0, _default_Model);
            
            _On_Model_Created?.Invoke(_default_Model);
            
            return;
        }
        
        GarageCarModel _saved_Model = Instantiate(_Player_Data._Car_Model, _Models_Position);
        
        _Models_On_Scene.Insert(_Models_On_Scene.Count - 1, _saved_Model);
        
        SceneMediator.SaveCar(_saved_Model.ShopData.Car, _saved_Model);

        _On_Model_Created?.Invoke(_saved_Model);
    }
    
    public void SwitchCar(int _value)
    {
        if(_Models_On_Scene.Count == 0)
            return;
        
        _Car_Index += _value;

        if (_Car_Index >= _Models_On_Scene.Count || _Models_On_Scene.Count == 1)
            _Car_Index = 0;
        else if (_Car_Index < 0)
            _Car_Index = _Models_On_Scene.Count - 1;

        DisableCars();

        GarageCarModel _selected_Model = _Models_On_Scene[_Car_Index];

        _selected_Model.gameObject.SetActive(true);
        
        GarageEvents.OnCarSwitched(_selected_Model);

        SceneMediator.PlayerData._Car_Model = _selected_Model.ShopData.Model;

        List<Car> _player_Cars = SceneMediator.PlayerData._Player_Cars;

        for (int i = 0; i < _player_Cars.Count; i++)
            if (_selected_Model.ShopData.Name == _player_Cars[i].ShopData.Name)
            {
                SceneMediator.SaveCar(_selected_Model.ShopData.Car, _selected_Model);

                SceneMediator.PlayerData.SaveCar(_selected_Model.ShopData.Car, _selected_Model.ShopData.Model);
                
                GameEvents.GameSaving();
                
                return;
            }
        
        GameEvents.GameSaving();
    }

    private void DisableCars()
    {
        if (_Models_On_Scene.Count == 0)
            return;

        for (int i = 0; i < _Models_On_Scene.Count; i++)
            _Models_On_Scene[i].gameObject.SetActive(false);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{
    [SerializeField] private Car[] _Cars;

    [SerializeField] private MenuUI _Menu_UI;
    [SerializeField] private CarsMenu _Cars_Menu;
    [SerializeField] private CustomizeMenu _Customize_Menu;
    
    private int _Current_Car_Index = 0;

    private Car _Selected_Car;

    private void OnEnable()
    {
        GarageEvents._On_Purchased += BuyCar;
    }

    private void Start()
    {
        SelectCar(_Current_Car_Index);
    }

    public void SelectCar(int _next_Value)
    {
        
        List<Car> _cars = SaveSystem._Player_Data._Cars;

        if (_Cars.Length == 1)
            _Current_Car_Index = 0;

        for (int i = 0; i < _Cars.Length; i++)
        {
            _Cars[i].gameObject.SetActive(false);
        }
        
        _Current_Car_Index += _next_Value;
        if (_Current_Car_Index >= _Cars.Length)
        {
            _Current_Car_Index = 0;
        }
        else if (_Current_Car_Index < 0)
        {
            _Current_Car_Index = _Cars.Length - 1;
        }

        _Selected_Car = _Cars[_Current_Car_Index];
        
        _Selected_Car.gameObject.SetActive(true);
        _Cars_Menu.CarInfo(_Selected_Car);
        
        
        for (int i = 0; i < _cars.Count; i++)
        {
            if (_cars[i].Data.Name == _Selected_Car.Data.Name)
            {
                SceneMediator.Car = _Selected_Car.Data.Prefab;
                _Customize_Menu.SelectCar(_Selected_Car);
            }
        }
    }

    private void BuyCar()
    {

        if (!EconomicTransactions.BuyCar(_Selected_Car))
        {
            _Menu_UI.NotEnoughMoneyText();
            return;
        }
        
        _Cars_Menu.CarInfo(_Selected_Car);
        
        SceneMediator.Car = _Selected_Car.Data.Prefab;
        _Customize_Menu.SelectCar(_Selected_Car);
        
        SaveSystem.Save();
        
    }

    private void OnDisable()
    {
        GarageEvents._On_Purchased -= BuyCar;
    }
}

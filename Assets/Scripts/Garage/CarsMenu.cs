using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CarsMenu : MonoBehaviour
{
    [SerializeField] private CarSelection _Car_Selection;
    
    [SerializeField] private TextMeshProUGUI _Name;
    [SerializeField] private TextMeshProUGUI _Price;
    [SerializeField] private TextMeshProUGUI _Max_Speed;
    [SerializeField] private TextMeshProUGUI _Drift_Force;

    [SerializeField] private Button _Next_Car;
    [SerializeField] private Button _Prev_Car;
    
    [SerializeField] private Button _Purchase_Button;
    [SerializeField] private Button _Customize_Button;

    private void Awake()
    {
        _Next_Car.onClick.AddListener(delegate { _Car_Selection.SelectCar(1); });
        _Prev_Car.onClick.AddListener(delegate { _Car_Selection.SelectCar(-1); });
            
        _Purchase_Button.onClick.AddListener(GarageEvents.OnPurchased);
    }
    

    public void CarInfo(Car _car)
    {
        List<Car> _cars = SaveSystem._Player_Data._Cars;

        _Purchase_Button.gameObject.SetActive(false);
        _Customize_Button.gameObject.SetActive(false);
        
        _Name.text = $"Name: {_car.Data.Name}";
        _Max_Speed.text = $"Max speed: {_car.Data.MaxSpeed}";
        _Drift_Force.text = $"Drift force: {_car.Data.DriftForce}";

        if(_car.Data.Price == 0)
        {
            _Price.text = "Price: Free";
        }
        else
        {
            _Price.text = $"Price: {_car.Data.Price}$";
            _Purchase_Button.gameObject.SetActive(true);
        }
        
        for (int i = 0; i < _cars.Count; i++)
        {
            if (_cars[i].Data.Name == _car.Data.Name)
            {
                _Price.text = "Price: Bought";
                _Purchase_Button.gameObject.SetActive(false);
                _Customize_Button.gameObject.SetActive(true);
            }
        }
    }
    
}
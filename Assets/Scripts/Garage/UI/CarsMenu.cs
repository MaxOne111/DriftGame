using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarsMenu : MonoBehaviour
{
    [SerializeField] private GarageCars _Garage_Cars;

    [SerializeField] private MenuUI _Menu_UI;
    
    [SerializeField] private TextMeshProUGUI _Name;
    [SerializeField] private TextMeshProUGUI _Price;
    [SerializeField] private TextMeshProUGUI _Max_Speed;
    [SerializeField] private TextMeshProUGUI _Drift_Force;

    [SerializeField] private Button _Next_Car;
    [SerializeField] private Button _Prev_Car;
    
    [SerializeField] private Button _Purchase_Button;
    [SerializeField] private Button _Customize_Button;

    private GarageCarModel _Selected_Car;

    private event Action _On_Car_Switched;
    
    public void Init()
    {
        _Garage_Cars._On_Model_Created += SelectCar;

        _Next_Car.onClick.AddListener(delegate { _Garage_Cars.SwitchCar(1); });
        _Prev_Car.onClick.AddListener(delegate { _Garage_Cars.SwitchCar(-1); });
        
        _Purchase_Button.onClick.AddListener(BuyCar);
    }

    private void Start()
    {
        CarInfo();
        AvailableCars();
    }

    private void OnEnable()
    {
        GarageEvents._On_Car_Switched += SelectCar;

        _On_Car_Switched += CarInfo;
        _On_Car_Switched += AvailableCars;
    }
    
    private void SelectCar(GarageCarModel _car)
    {
        _Selected_Car = _car;
        _On_Car_Switched?.Invoke();
    }

    private void AvailableCars()
    {
        List<Car> _cars = SceneMediator.PlayerData._Player_Cars;

        _Purchase_Button.gameObject.SetActive(true);
        _Customize_Button.interactable = false;

        if (_cars.Count == 0)
            return;

        for (int i = 0; i < _cars.Count; i++)
            if (_cars[i].ShopData.Name == _Selected_Car.ShopData.Name)
            {
                _Price.text = "Price: Bought";
                _Purchase_Button.gameObject.SetActive(false);
                _Customize_Button.interactable = true;
            }
    }
    private void CarInfo()
    {
        _Name.text = $"Name: {_Selected_Car.ShopData.Name}";
        _Max_Speed.text = $"Max speed: {_Selected_Car.Characteristics.MaxSpeed}";
        _Drift_Force.text = $"Drift force: {_Selected_Car.Characteristics.DriftForce}";
        
        _Price.text = $"Price: {_Selected_Car.ShopData.Price}$";
    }
    
    private void BuyCar()
    {
        if (!CarPurchase.TryBuyCar(_Selected_Car.ShopData.Price, SceneMediator.PlayerData._Money))
        {
            _Menu_UI.NotEnoughMoneyText();
            return;
        }
        
        CarPurchase.BuyCar(_Selected_Car.ShopData.Car, SceneMediator.PlayerData._Player_Cars);
        
        SceneMediator.SaveCar(_Selected_Car.ShopData.Car, _Selected_Car);
        
        SceneMediator.PlayerData.SaveCar(_Selected_Car.ShopData.Car, _Selected_Car.ShopData.Model);
        
        GarageEvents.OnPurchased();
        _On_Car_Switched?.Invoke();

        GameEvents.GameSaving();
    }

    private void OnDisable()
    {
        GarageEvents._On_Car_Switched -= SelectCar;
        
        _On_Car_Switched -= CarInfo;
        _On_Car_Switched -= AvailableCars;
    }

    private void OnDestroy()
    {
        _Garage_Cars._On_Model_Created -= SelectCar;
        
        _Next_Car.onClick.RemoveListener(delegate { _Garage_Cars.SwitchCar(1); });
        _Prev_Car.onClick.RemoveListener(delegate { _Garage_Cars.SwitchCar(-1); });
        
        _Purchase_Button.onClick.RemoveListener(BuyCar);
    }
}
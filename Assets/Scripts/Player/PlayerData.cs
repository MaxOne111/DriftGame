using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

[Serializable]
public class PlayerData
{
    public int _Money;
    public List<Car> _Player_Cars;
    public Car _Saved_Car;
    public GarageCarModel _Car_Model;

    public void SaveCar(Car _car, GarageCarModel _model)
    {
        _Saved_Car = _car;
        _Car_Model = _model;
    }

}
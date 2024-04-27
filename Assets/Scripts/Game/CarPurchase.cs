using System.Collections.Generic;
using UnityEngine;

public static class CarPurchase
{
    public static bool TryBuyCar(int _price, int _player_Money)
    {
        if (_price > _player_Money)
        {
            Debug.Log("Not enough");
            return false;
        }
        return true;
    }
    
    public static void BuyCar(Car _car, List<Car> _player_Cars)
    {
        CashTransactions.SpendMoney(_car.ShopData.Price);
        _player_Cars.Add(_car);
    }
}
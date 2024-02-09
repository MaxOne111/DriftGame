using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EconomicTransactions
{
    
    public static void AddMoney(int _value)
    {
        if (_value > 0)
            SaveSystem._Player_Data._Money += _value;
    }

    public static void SpendMoney(int _value)
    {
        if (_value > SaveSystem._Player_Data._Money && _value > 0)
            return;
        
        SaveSystem._Player_Data._Money -= _value;
    }

    public static bool BuyCar(Car _car)
    {
        if (_car.Data.Price > SaveSystem._Player_Data._Money)
        {
            Debug.Log("Not enough");
            return false;
        }
        
        SpendMoney(_car.Data.Price);
        SaveSystem._Player_Data._Cars.Add(_car);

        return true;
    }
}

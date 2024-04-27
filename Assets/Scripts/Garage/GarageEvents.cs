using System;

public static class GarageEvents
{
    public static event Action _On_Purchased;
    public static event Action<GarageCarModel> _On_Car_Switched;

    public static void OnCarSwitched(GarageCarModel _car) => _On_Car_Switched?.Invoke(_car);

    public static void OnPurchased() => _On_Purchased?.Invoke();
}

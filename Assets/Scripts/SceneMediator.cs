public static class SceneMediator
{
    public static Car SelectedCar { get; private set; }

    public static GarageCarModel SelectedModel { get; private set; }
    public  static PlayerData PlayerData { get; set; }
    public static int LevelIndex { get; set; }
    
    public static bool IsPhoton { get; set; }
    public static bool IsHost { get; set; }

    public static void SaveCar(Car _car, GarageCarModel _model)
    {
        SelectedCar = _car;
        SelectedModel = _model;
    }
}
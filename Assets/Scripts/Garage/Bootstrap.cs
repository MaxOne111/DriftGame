using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private SaveSystem _Save_System;
    [SerializeField] private CarsMenu _Cars_Menu;
    [SerializeField] private GarageCars _Garage_Cars;

    private void Awake() => StartGame();

    private void StartGame()
    {
        _Save_System.Init();
        _Garage_Cars.Init();
        _Cars_Menu.Init();

        _Garage_Cars.CreateModels();
    }
}

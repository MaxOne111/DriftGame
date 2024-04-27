using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _Main_Menu;
    [SerializeField] private GameObject _Cars_Menu;
    [SerializeField] private GameObject _Customize_Menu;
    
    [SerializeField] private Button _Menu_Button;
    [SerializeField] private Button _Cars_Button;
    [SerializeField] private Button _Customize_Button;

    [SerializeField] private TextMeshProUGUI _Money;
    

    [SerializeField] private CameraMovement _Camera;

    [SerializeField] private TextMeshProUGUI _Need_Car_Text;
    [SerializeField] private TextMeshProUGUI _Not_Enough_Money_Text;

    private void OnEnable()
    {
        _Menu_Button.onClick.AddListener(SelectMenu);
        _Cars_Button.onClick.AddListener(SelectCars);
        _Customize_Button.onClick.AddListener(CustomizeMenu);

        GarageEvents._On_Purchased += CurrentMoney;
    }

    private void Start()
    {
        CurrentMoney();
    }

    private void SelectCars()
    {
        _Main_Menu.SetActive(false);
        _Cars_Menu.SetActive(true);
        _Camera.WatchCar();
    }

    private void SelectMenu()
    {
        _Main_Menu.SetActive(true);
        _Cars_Menu.SetActive(false);
        _Camera.StartPoint();
    }
    
    private void CustomizeMenu()
    {
        _Customize_Menu.SetActive(true);
        _Main_Menu.SetActive(false);
        _Camera.WatchCar();
    }

    private void CurrentMoney() => _Money.text = $"{SceneMediator.PlayerData._Money}$";

    public void NeedCarText()
    {
        _Need_Car_Text.DOFade(1, 0).SetLink(gameObject);
        _Need_Car_Text.DOFade(0, 4f).SetLink(gameObject);
    }
    
    public void NotEnoughMoneyText()
    {
        _Not_Enough_Money_Text.DOFade(1, 0).SetLink(gameObject);
        _Not_Enough_Money_Text.DOFade(0, 4f).SetLink(gameObject);
    }

    private void OnDisable()
    {
        _Menu_Button.onClick.RemoveListener(SelectMenu);
        _Cars_Button.onClick.RemoveListener(SelectCars);

        GarageEvents._On_Purchased -= CurrentMoney;
    }
}
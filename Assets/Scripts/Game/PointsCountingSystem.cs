using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class PointsCountingSystem : MonoBehaviour
{
    [SerializeField] private GameUI _Game_UI;

    public static Action _On_Drift_Beginning;
    public static Action _On_Drift_Ending;

    private int _Drift_Points = 0;
    private int _Total_Points = 0;

    private bool _Stop_Counting;

    private int _Added_Money;

    private void OnEnable()
    {
        _On_Drift_Beginning += StartPointsCounting;
        _On_Drift_Ending += EndPointsCounting;

        GameEvents._On_Level_Ended += InvokeTotalCounting;
    }

    public static void OnDriftBeginning()
    {
        _On_Drift_Beginning?.Invoke();
    }

    public static void OnDriftEnding()
    {
        _On_Drift_Ending?.Invoke();
    }

    private void StartPointsCounting()
    {
        _Drift_Points += 1;
        _Game_UI.PointsCount(_Drift_Points);
    }

    private void EndPointsCounting()
    {
        _Total_Points += _Drift_Points;
        _Drift_Points = 0;
    }

    private void InvokeTotalCounting()
    {
        StartCoroutine(TotalCounting());
    }
    
    private IEnumerator TotalCounting()
    {
        if (_Total_Points == 0)
            yield break; 
        
        float _delay = 1.5f;

        int _added_Money = 0;

        int _total_Temp = _Total_Points;
        
        _Game_UI.TotalPoints(_Total_Points);
        
        yield return new WaitForSeconds(_delay);
        
        while (_total_Temp > 0)
        {
            if (_Stop_Counting)
                yield break;

            _total_Temp -= 1;
            if (_total_Temp % 2 == 0)
                _added_Money += 1;
            
            _Game_UI.TotalPoints(_total_Temp);
            _Game_UI.AddedMoney(_added_Money);

            yield return null;
        }

        _Total_Points = 0;

        _Added_Money = _added_Money;
        
        CashTransactions.AddMoney(_added_Money);
        GameEvents.GameSaving();
    }

    public void ExtraMoney()
    {
        CashTransactions.AddMoney(_Added_Money);
        GameEvents.GameSaving();
    }

    public void FastTotalCounting()
    {
        _Stop_Counting = true;

        if (_Total_Points == 0)
            return;
        
        int _added_Money =  _Total_Points / 2;

        _Total_Points = 0;
        
        _Added_Money = _added_Money;
        
        _Game_UI.TotalPoints(_Total_Points);
        _Game_UI.AddedMoney(_added_Money);
        
        CashTransactions.AddMoney(_added_Money);
        GameEvents.GameSaving();
    }

    private void OnDisable()
    {
        _On_Drift_Beginning -= StartPointsCounting;
        _On_Drift_Ending -= EndPointsCounting;
        
        GameEvents._On_Level_Ended -= InvokeTotalCounting;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
   [SerializeField] private FixedJoystick _Joystick;
   [SerializeField] private DriftButton _Drift_Button;
      
   [SerializeField] private TextMeshProUGUI _Points_Text;
   [SerializeField] private TextMeshProUGUI _Remaining_Time;
   
   [SerializeField] private GameObject _Finish_Level_Panel;
   
   [SerializeField] private TextMeshProUGUI _Added_Money;
   [SerializeField] private TextMeshProUGUI _Total_Points;

   [SerializeField] private Button _To_Main_Menu;

   [SerializeField] private Button _Rewarded_Button;
   public FixedJoystick Joystick{get=>_Joystick;}
   public DriftButton DriftButton{get=>_Drift_Button;}
   
   public Button RewardedButton{get=>_Rewarded_Button;}

   private void OnEnable()
   {
      PointsCountingSystem._On_Drift_Beginning += ShowPoints;
      PointsCountingSystem._On_Drift_Ending += HidePoints;

      
      
      _To_Main_Menu.onClick.AddListener(ToMainMenu);

      if (SceneMediator.IsPhoton)
         _To_Main_Menu.gameObject.SetActive(false);
      
      GameEvents._On_Level_Ended += ShowFinishPanel;
   }

   private void ShowPoints()
   {
      _Points_Text.gameObject.SetActive(true);
   }

   private void HidePoints()
   {
      _Points_Text.gameObject.SetActive(false);
   }

   public void PointsCount(int _points)
   {
      _Points_Text.text = $"{_points}";
   }
   
   public void RemainingTime(float _sec, float _min)
   {
      _Remaining_Time.text = $"{_min:00}:{_sec:00}";
   }

   public void AddedMoney(int _value)
   {
      _Added_Money.text = $"{_value}$";
   }

   public void TotalPoints(float _points)
   {
      _Total_Points.text = $"{_points}";
   }

   private void ShowFinishPanel()
   {
      _Finish_Level_Panel.SetActive(true);
      
      _Finish_Level_Panel.transform.localScale = Vector3.zero;

      _Finish_Level_Panel.transform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutElastic);
      
      if (!IronSource.Agent.isRewardedVideoAvailable())
         _Rewarded_Button.interactable = false;
   }

   private void ToMainMenu()
   {
      SceneManager.LoadScene(0);
   }

   private void OnDisable()
   {
      PointsCountingSystem._On_Drift_Beginning -= ShowPoints;
      PointsCountingSystem._On_Drift_Ending -= HidePoints;
      
      GameEvents._On_Level_Ended -= ShowFinishPanel;
      
      _To_Main_Menu.onClick.RemoveListener(ToMainMenu);
   }
}

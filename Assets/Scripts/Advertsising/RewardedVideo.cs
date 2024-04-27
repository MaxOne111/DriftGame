using UnityEngine;


public class RewardedVideo : MonoBehaviour
{
    [SerializeField] private GameUI _Game_UI;
    [SerializeField] private PointsCountingSystem _Points_System;

    private void Awake()
    {
        _Game_UI.RewardedButton.onClick.AddListener(ShowRewarded);
    }

    private void OnEnable()
    {
        EventsInit();
    }

    private void EventsInit()
    {
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

    }

    private bool VideoIsReady() => IronSource.Agent.isRewardedVideoAvailable();

    private void ShowRewarded()
    {
        if (VideoIsReady())
            IronSource.Agent.showRewardedVideo();
        else
            Debug.Log("Not ready");

    }
    

    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo) 
    {
        Debug.Log("Available");
    }

    void RewardedVideoOnAdUnavailable() 
    {
        Debug.Log("Unavailable");
    }

    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo){
    }

    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("Get reward");
        _Points_System.ExtraMoney();
        
    }

    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo){
    }

    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo){
    }

    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo){
    }

}
    

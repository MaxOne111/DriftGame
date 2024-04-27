using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceInit : MonoBehaviour
{
    public string _App_Key;

    private void OnApplicationPause(bool pauseStatus) => IronSource.Agent.onApplicationPause(pauseStatus);

    private void Awake() => Init();


    private void Init()
    {
        RewardedInit();
        
        IronSource.Agent.validateIntegration();
        
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
    }

    private void RewardedInit()
    {
        IronSource.Agent.init(_App_Key, IronSourceAdUnits.REWARDED_VIDEO);
        IronSource.Agent.shouldTrackNetworkState(true);
    }

    private void SdkInitializationCompletedEvent()
    {
        Debug.Log("Initialization was successful");
    }

    private void OnDestroy()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent -= SdkInitializationCompletedEvent;
    }
}

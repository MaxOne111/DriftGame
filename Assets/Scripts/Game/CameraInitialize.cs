using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraInitialize : MonoBehaviour
{
    private CinemachineVirtualCamera _Camera;

    private void Awake()
    {
        _Camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Init(Transform _car)
    {
        _Camera.Follow = _car;
        _Camera.LookAt = _car;
    }
}

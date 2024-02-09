using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _Watch_Car_Point;
    [SerializeField] private Transform _Car_Point;
    [SerializeField] private Transform _Start_Point;


    public void WatchCar()
    {
        StartCoroutine(MovementToWatchPoint());
    }

    public void StartPoint()
    {
        StartCoroutine(MovementToStartPoint());
    }
    
    private IEnumerator MovementToWatchPoint()
    {
        transform.DOMove(_Watch_Car_Point.position, 1.5f).SetEase(Ease.OutCubic).SetLink(gameObject);
        while (transform.position != _Watch_Car_Point.position)
        {
            transform.LookAt(_Car_Point);
            yield return null;
        }
    }
    
    private IEnumerator MovementToStartPoint()
    {
        transform.DOMove(_Start_Point.position, 1.5f).SetEase(Ease.OutCubic).SetLink(gameObject);
        while (transform.position != _Start_Point.position)
        {
            transform.LookAt(_Car_Point);
            yield return null;
        }
    }
}

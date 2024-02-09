using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointsCountingMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PointsCountingSystem _Points_Counting;

    public void OnPointerClick(PointerEventData eventData)
    {
        _Points_Counting.FastTotalCounting();
    }
}

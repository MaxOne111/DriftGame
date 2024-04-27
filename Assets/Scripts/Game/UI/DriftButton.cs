using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DriftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsTouched { get; set; }

    public void OnPointerDown(PointerEventData eventData) => IsTouched = true;

    public void OnPointerUp(PointerEventData eventData) => IsTouched = false;
}

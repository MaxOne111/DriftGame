using System;
using UnityEngine;

public class LevelsMenu : MonoBehaviour
{
    [SerializeField] private LevelButton[] _Level_Buttons;
    public PhotonRoom Photon { get; set; }

    private void Start()
    {
        Photon = null;
    }

    public void PhotonTypeLoad(PhotonRoom _photon)
    {
        for (int i = 0; i < _Level_Buttons.Length; i++)
        {
            _Level_Buttons[i].Photon = _photon;
        }
    }

    private void OnDisable()
    {
        Photon = null;
    }
}


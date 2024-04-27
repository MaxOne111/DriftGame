using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private MenuUI _Menu_UI;
    [SerializeField] private int _Level_Index;
    private Button _Button;
    
    public PhotonRoom Photon { get; set; }

    private void Awake()
    {
        _Button = GetComponent<Button>();
    }

    private void OnEnable() => _Button.onClick.AddListener(delegate { SelectLevel(Photon); });
    
    private void SelectLevel(PhotonRoom _photon)
    {
        LevelLoader _loader = new LevelLoader();

        if (!SceneMediator.SelectedCar)
        {
            _Menu_UI.NeedCarText();
            return;
        }
            

        if (_photon)
            _loader.LoadLevel(_Level_Index, _photon);
        else
        {
            _loader.LoadLevel(_Level_Index);
            
        }
        
    }
}
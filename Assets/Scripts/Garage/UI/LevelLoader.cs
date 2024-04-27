using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader
{
    public void LoadLevel(int _index)
    {
        SceneMediator.LevelIndex = _index;
        SceneManager.LoadScene(1);
    }

    public void LoadLevel(int _index, PhotonRoom _photon)
    {
        SceneMediator.LevelIndex = _index;
        _photon.CreateRoom();
    }
    
}
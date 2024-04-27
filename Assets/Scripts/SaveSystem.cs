using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static string _Save_Filename = "_playerData.json";
    
    private static string _Path_To_Save_File;

    public PlayerData _Player_Data;
    
    private void DefaultData()
    {
        _Player_Data = new PlayerData
        {
            _Money = 15000,
            _Player_Cars = new List<Car>(),
            _Car_Model = null,
            _Saved_Car = null,
        };
        
        Save();
    }

    public void Init()
    {
        GameEvents._Game_Saving += Save;
        
        _Path_To_Save_File = Path.Combine(Application.persistentDataPath, _Save_Filename);
        DetectSaveFile();
        
        SceneMediator.PlayerData = _Player_Data;
    }
    
    private void DetectSaveFile()
    {
        if (File.Exists(_Path_To_Save_File))
        {
            string _json = File.ReadAllText(_Path_To_Save_File);
            
            _Player_Data = JsonUtility.FromJson<PlayerData>(_json);

            GameEvents.OnGameLoaded();
        }
        else
            DefaultData();
        
        Debug.Log(_Player_Data._Player_Cars);
    }

    private void Save()
    {
        string _json = JsonUtility.ToJson(_Player_Data);
        File.WriteAllText(_Path_To_Save_File, _json);
    } 
}
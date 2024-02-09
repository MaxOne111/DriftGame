using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;


public class SaveSystem : MonoBehaviour
{
    private string _Save_Filename = "_playerData.dot";
    
    private static string _Path_To_Save_File;

    private int _Money;

    public static PlayerData _Player_Data;

    private void Awake()
    {
        _Path_To_Save_File = Path.Combine(Application.persistentDataPath, _Save_Filename);
        DetectSaveFile();
    }

    private void FirstGame()
    {
        _Player_Data._Money = 500;
        _Player_Data._Cars = new List<Car>();
        Save();
    }

    private void DetectSaveFile()
    {
        if (File.Exists(_Path_To_Save_File))
        {
            _Player_Data = JsonSerializer.Deserialize<PlayerData>(_Path_To_Save_File);
        }
        else
        {
            FirstGame();
        }
    }

    public static void Save()
    {
        JsonSerializer.Serialize(_Path_To_Save_File, _Player_Data);
    }
}

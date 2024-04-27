using System;
using Photon.Pun;

public static class GameEvents
{
    public static Action _On_Level_Started;
    public static Action _On_Level_Ended;
    public static Action _On_Game_Loaded;
    public static Action _Game_Saving;

    public static void OnLevelStarted() => _On_Level_Started?.Invoke();

    [PunRPC]
    public static void OnLevelEnded() => _On_Level_Ended?.Invoke();

    public static void OnGameLoaded() => _On_Game_Loaded?.Invoke();

    public static void GameSaving() => _Game_Saving?.Invoke();
}

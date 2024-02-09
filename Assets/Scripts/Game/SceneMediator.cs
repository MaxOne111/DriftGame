using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneMediator
{
    public static Car Car { get; set; }
    public static int LevelIndex { get; set; }
    
    public static bool IsPhoton { get; set; }
    public static bool IsHost { get; set; }
}

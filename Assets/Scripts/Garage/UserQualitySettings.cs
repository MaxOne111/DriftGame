using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserQualitySettings : MonoBehaviour
{
    public void SetQuality(int _index) => UnityEngine.QualitySettings.SetQualityLevel(_index);
}

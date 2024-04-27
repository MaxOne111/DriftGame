using UnityEngine;


[CreateAssetMenu(menuName = "CarCustomizeData")]
public class CarCustomizeData : ScriptableObject
{
    [SerializeField] private Material[] _Colors;
    [SerializeField] private GameObject[] _Spoilers;
    [SerializeField] private GameObject[] _Bumpers;
    [SerializeField] private GameObject[] _Wheels;
    public Material[] Colors => _Colors;
    public GameObject[] Spoilers => _Spoilers;
    public GameObject[] Bumpers => _Bumpers;
    public GameObject[] Wheels => _Wheels;


}
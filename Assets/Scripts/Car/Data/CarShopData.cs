using UnityEngine;

[CreateAssetMenu(menuName = "CarShopData")]
public class CarShopData : ScriptableObject
{
    [SerializeField] private string _Name;
    [SerializeField] private int _Price;
    [SerializeField] private Car _Car;
    [SerializeField] private GarageCarModel _Model;
    public string Name => _Name;
    public int Price => _Price;
    public Car Car => _Car;
    public GarageCarModel Model => _Model;


}

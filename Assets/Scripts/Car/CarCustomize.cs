using UnityEngine;
using Object = UnityEngine.Object;

public class CarCustomize
{
    public void InstallColor(Renderer _renderer, Material _color) => _renderer.material = _color;

    public GameObject InstallOnCar(GameObject _prefab, Transform _place)
    {
        GameObject _part = Object.Instantiate(_prefab, _place.position, _place.rotation, _place);

        return _part;
    }

    public void UninstallFromCar(GameObject _part)
    {
        if (_part == null)
            return;
        
        Object.Destroy(_part);
    }
}
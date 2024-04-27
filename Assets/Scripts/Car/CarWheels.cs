using System.Collections.Generic;
using UnityEngine;

public class CarWheels : MonoBehaviour
{
    [SerializeField] private Vector3 _Body_Mass_Center; 
                                    
    [SerializeField] private GameObject _Front_Left_Mesh;
    [SerializeField] private WheelCollider _Front_Left_Collider;

    [SerializeField] private GameObject _Front_Right_Mesh;
    [SerializeField] private WheelCollider _Front_Right_Collider;

    [SerializeField] private GameObject _Rear_Left_Mesh;
    [SerializeField] private WheelCollider _Rear_Left_Collider;

    [SerializeField] private GameObject _Rear_Right_Mesh;
    [SerializeField] private WheelCollider _Rear_Right_Collider;
    
    public Vector3 BodyMassCenter => _Body_Mass_Center;

    public GameObject FrontLeftMesh => _Front_Left_Mesh;
    public WheelCollider FrontLeftCollider => _Front_Left_Collider;

    public GameObject FrontRightMesh => _Front_Right_Mesh ;
    public WheelCollider FrontRightCollider => _Front_Right_Collider;

    public GameObject RearLeftMesh => _Rear_Left_Mesh;
    public WheelCollider RearLeftCollider => _Rear_Left_Collider;

    public GameObject RearRightMesh => _Rear_Right_Mesh;
    public WheelCollider RearRightCollider => _Rear_Right_Collider;

    public void InitMeshes(GameObject[] _meshes)
    {
        _Front_Left_Mesh = _meshes[0];
        _Front_Right_Mesh = _meshes[1];
        _Rear_Left_Mesh = _meshes[2];
        _Rear_Right_Mesh = _meshes[3];
    }
}
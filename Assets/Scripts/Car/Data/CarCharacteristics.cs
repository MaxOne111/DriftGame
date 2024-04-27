using UnityEngine;

[CreateAssetMenu(menuName = "CarCharacteristics")]
public class CarCharacteristics : ScriptableObject
    {
        [SerializeField] private float _Max_Speed;
        [SerializeField] private float _Drift_Force;
        [SerializeField] private float _Max_Reverse_Speed = 45;
        [SerializeField] private float _Acceleration_Multiplier = 2;
        [SerializeField] private float _Max_Steering_Angle = 27;
        [SerializeField] private float _Steering_Speed = 0.5f;
        [SerializeField] private float _Brake_Force = 350;
        [SerializeField] private float _Deceleration_Multiplier = 2;
        
        public float MaxSpeed => _Max_Speed;
        public float DriftForce => _Drift_Force;
        public float MaxReverseSpeed => _Max_Reverse_Speed;
        public float AccelerationMultiplier => _Acceleration_Multiplier;
        public float MaxSteeringAngle => _Max_Steering_Angle;
        public float SteeringSpeed => _Steering_Speed;
        public float BrakeForce => _Brake_Force;
        public float DecelerationMultiplier => _Deceleration_Multiplier;
     
        
    }
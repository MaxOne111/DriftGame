using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
      [SerializeField] private CarCharacteristics _Characteristics;
      
      [SerializeField] private ParticleSystem _RearLeft_Wheel_ParticleSystem;
      [SerializeField] private ParticleSystem _Rear_Right_Wheel_ParticleSystem;
      
      [SerializeField] private TrailRenderer _Rear_Left_Wheel_Tire_Skid;
      [SerializeField] private TrailRenderer _Rear_Right_Wheel_Tire_Skid;

      private CarWheels _Car_Wheels;

      private FixedJoystick _Joystick;
      private DriftButton _Drift_Button;

      private GameUI _Game_UI;
      
      private Rigidbody _Rigidbody;
      
      private PhotonView _Photon_View;

      private bool _Is_Can_Move = true;
      private bool _Is_Drifting;
      private bool _Is_Traction_Locked;
      private bool _Is_Decelerate_Car;
      
      private float _Car_Speed;
      private float _Steering_Sensitivity = 10;
      private float _Steering_Axis; 
      private float _Throttle_Axis; 
      private float _Drifting_Axis;
      private float _Local_Velocity_Z;
      private float _Local_Velocity_X;

      private WheelFrictionCurve _Front_Left_Wheel_Friction;
      private float _Front_Left_Wherl_ExtremumSlip;
      
      private WheelFrictionCurve _Front_Right_Wheel_Friction;
      private float _Front_Right_Wherl_ExtremumSlip;
      
      private WheelFrictionCurve _Rear_Left_Wheel_Friction;
      private float _Rear_Left_Wherl_ExtremumSlip;
      
      private WheelFrictionCurve _Rear_Right_Wheel_Friction;
      private float _Rear_Right_Wherl_ExtremumSlip;
      
      private void Awake()
      {
        _Rigidbody = GetComponent<Rigidbody>();
        _Photon_View = GetComponent<PhotonView>();
      }

      private void OnEnable()
      {
        GameEvents._On_Level_Ended += DisableMovement;
        
        _Game_UI = GameObject.FindWithTag("GameUI").GetComponent<GameUI>();

        _Joystick = _Game_UI.Joystick;
        _Drift_Button = _Game_UI.DriftButton;
      }

    private void Start()
    {
      if (SceneMediator.IsPhoton && !_Photon_View.IsMine)
        return;
      
      _Rigidbody.centerOfMass = _Car_Wheels.BodyMassCenter;

      WheelsSetup();
    }
    
    
    private void FixedUpdate()
    {
      if (SceneMediator.IsPhoton && !_Photon_View.IsMine)
        return;
      
      _Car_Speed = 2f * Mathf.PI * _Car_Wheels.FrontLeftCollider.radius * _Car_Wheels.FrontLeftCollider.rpm * 60f / 1000f;

      _Local_Velocity_X = transform.InverseTransformDirection(_Rigidbody.velocity).x;

      _Local_Velocity_Z = transform.InverseTransformDirection(_Rigidbody.velocity).z;

      AnimateWheelMeshes();

    }
    
    private void Update()
    {
      if (SceneMediator.IsPhoton && !_Photon_View.IsMine)
        return;
      InputSystem();
    }

    public void WheelsInit() => _Car_Wheels = GetComponent<CarWheels>();

    private void DisableMovement()
    {
      _Is_Can_Move = false;
      InvokeRepeating("DecelerateCar", 0f, 0.1f);
    }

    private void WheelsSetup()
    {
      _Front_Left_Wheel_Friction = new WheelFrictionCurve ();
      WheelFrictionCurve _sideways_Friction_FL = _Car_Wheels.FrontLeftCollider.sidewaysFriction;
      
      _Front_Left_Wheel_Friction.extremumSlip = _sideways_Friction_FL.extremumSlip;
        _Front_Left_Wherl_ExtremumSlip = _sideways_Friction_FL.extremumSlip;
        _Front_Left_Wheel_Friction.extremumValue = _sideways_Friction_FL.extremumValue;
        _Front_Left_Wheel_Friction.asymptoteSlip = _sideways_Friction_FL.asymptoteSlip;
        _Front_Left_Wheel_Friction.asymptoteValue = _sideways_Friction_FL.asymptoteValue;
        _Front_Left_Wheel_Friction.stiffness = _sideways_Friction_FL.stiffness;

        WheelFrictionCurve _sideways_Friction_FR = _Car_Wheels.FrontRightCollider.sidewaysFriction;
        
        _Front_Right_Wheel_Friction.extremumSlip = _sideways_Friction_FR.extremumSlip;
        _Front_Right_Wherl_ExtremumSlip = _sideways_Friction_FR.extremumSlip;
        _Front_Right_Wheel_Friction.extremumValue = _sideways_Friction_FR.extremumValue;
        _Front_Right_Wheel_Friction.asymptoteSlip = _sideways_Friction_FR.asymptoteSlip;
        _Front_Right_Wheel_Friction.asymptoteValue = _sideways_Friction_FR.asymptoteValue;
        _Front_Right_Wheel_Friction.stiffness = _sideways_Friction_FR.stiffness;
        
      _Rear_Left_Wheel_Friction = new WheelFrictionCurve ();
      WheelFrictionCurve _sideways_Friction_RL = _Car_Wheels.RearLeftCollider.sidewaysFriction;
      
      _Rear_Left_Wheel_Friction.extremumSlip = _sideways_Friction_RL.extremumSlip;
        _Rear_Left_Wherl_ExtremumSlip = _sideways_Friction_RL.extremumSlip;
        _Rear_Left_Wheel_Friction.extremumValue = _sideways_Friction_RL.extremumValue;
        _Rear_Left_Wheel_Friction.asymptoteSlip = _sideways_Friction_RL.asymptoteSlip;
        _Rear_Left_Wheel_Friction.asymptoteValue = _sideways_Friction_RL.asymptoteValue;
        _Rear_Left_Wheel_Friction.stiffness = _sideways_Friction_RL.stiffness;
        
      _Rear_Right_Wheel_Friction = new WheelFrictionCurve ();
      WheelFrictionCurve _sideways_Friction_RR = _Car_Wheels.RearRightCollider.sidewaysFriction;
      
      _Rear_Right_Wheel_Friction.extremumSlip = _sideways_Friction_RR.extremumSlip;
        _Rear_Right_Wherl_ExtremumSlip = _sideways_Friction_RR.extremumSlip;
        _Rear_Right_Wheel_Friction.extremumValue = _sideways_Friction_RR.extremumValue;
        _Rear_Right_Wheel_Friction.asymptoteSlip = _sideways_Friction_RR.asymptoteSlip;
        _Rear_Right_Wheel_Friction.asymptoteValue = _sideways_Friction_RR.asymptoteValue;
        _Rear_Right_Wheel_Friction.stiffness = _sideways_Friction_RR.stiffness;
    }

    private void InputSystem()
    {
      if (!_Is_Can_Move)
        return;

      if(_Joystick.Vertical > 0)
      {
        CancelInvoke("DecelerateCar");
        _Is_Decelerate_Car = false;
        GoForward();
      }
      
      if(_Joystick.Vertical < 0)
      {
        CancelInvoke("DecelerateCar");
        _Is_Decelerate_Car = false;
        GoReverse();
      }
      
      if(_Joystick.Horizontal < 0)
        TurnLeft();

      if(_Joystick.Horizontal > 0)
        TurnRight();

      if(_Drift_Button.IsTouched)
      {
        CancelInvoke("DecelerateCar");
        _Is_Decelerate_Car = false;
        Handbrake();
      }
      
      if(!_Drift_Button.IsTouched)
        RecoverTraction();

      if(_Joystick.Vertical == 0)
        ThrottleOff();

      if(_Joystick.Vertical == 0  && !_Drift_Button.IsTouched && !_Is_Decelerate_Car)
      {
        InvokeRepeating("DecelerateCar", 0f, 0.1f);
        _Is_Decelerate_Car = true;
      }
      
      if(_Joystick.Horizontal == 0 && _Steering_Axis != 0f)
        ResetSteeringAngle();
    }
  
    
    private void TurnLeft()
    {
      _Steering_Axis -= Time.deltaTime * _Steering_Sensitivity * _Characteristics.SteeringSpeed;
      
      if(_Steering_Axis < -1f)
        _Steering_Axis = -1f;
      
      float steeringAngle = _Steering_Axis * _Characteristics.MaxSteeringAngle;
      _Car_Wheels.FrontLeftCollider.steerAngle = Mathf.Lerp(_Car_Wheels.FrontLeftCollider.steerAngle, steeringAngle, _Characteristics.SteeringSpeed);
      _Car_Wheels.FrontRightCollider.steerAngle = Mathf.Lerp(_Car_Wheels.FrontRightCollider.steerAngle, steeringAngle, _Characteristics.SteeringSpeed);
    }

    private void TurnRight()
    {
      _Steering_Axis += Time.deltaTime * _Steering_Sensitivity * _Characteristics.SteeringSpeed;
      
      if(_Steering_Axis > 1f)
        _Steering_Axis = 1f;
      
      float steeringAngle = _Steering_Axis * _Characteristics.MaxSteeringAngle;
      _Car_Wheels.FrontLeftCollider.steerAngle = Mathf.Lerp(_Car_Wheels.FrontLeftCollider.steerAngle, steeringAngle, _Characteristics.SteeringSpeed);
      _Car_Wheels.FrontRightCollider.steerAngle = Mathf.Lerp(_Car_Wheels.FrontRightCollider.steerAngle, steeringAngle, _Characteristics.SteeringSpeed);
    }
    
    private void ResetSteeringAngle()
    {
      if(_Steering_Axis < 0f)
        _Steering_Axis += Time.deltaTime * _Steering_Sensitivity * _Characteristics.SteeringSpeed;
      
      else if(_Steering_Axis > 0f)
        _Steering_Axis -= Time.deltaTime * _Steering_Sensitivity * _Characteristics.SteeringSpeed;
      
      if(Mathf.Abs(_Car_Wheels.FrontLeftCollider.steerAngle) < 1f)
        _Steering_Axis = 0f;
      
      float steeringAngle = _Steering_Axis * _Characteristics.MaxSteeringAngle;
      _Car_Wheels.FrontLeftCollider.steerAngle = Mathf.Lerp(_Car_Wheels.FrontLeftCollider.steerAngle, steeringAngle, _Characteristics.SteeringSpeed);
      _Car_Wheels.FrontRightCollider.steerAngle = Mathf.Lerp(_Car_Wheels.FrontRightCollider.steerAngle, steeringAngle, _Characteristics.SteeringSpeed);
    }
    
    private void AnimateWheelMeshes()
    {
      _Car_Wheels.FrontLeftCollider.GetWorldPose(out Vector3 _wPosition_FL, out Quaternion _wRotation_FL);
      _Car_Wheels.FrontLeftMesh.transform.position = _wPosition_FL;
      _Car_Wheels.FrontLeftMesh.transform.rotation = _wRotation_FL;

      _Car_Wheels.FrontRightCollider.GetWorldPose(out Vector3 _wPosition_FR, out Quaternion _wRotation_FR);
      _Car_Wheels.FrontRightMesh.transform.position = _wPosition_FR;
      _Car_Wheels.FrontRightMesh.transform.rotation = _wRotation_FR;

      _Car_Wheels.RearLeftCollider.GetWorldPose(out Vector3 _wPosition_RL, out Quaternion _wRotation_RL);
      _Car_Wheels.RearLeftMesh.transform.position = _wPosition_RL;
      _Car_Wheels.RearLeftMesh.transform.rotation = _wRotation_RL;

      _Car_Wheels.RearRightCollider.GetWorldPose(out Vector3 _wPosition_RR, out Quaternion _wRotation_RR);
      _Car_Wheels.RearRightMesh.transform.position = _wPosition_RR;
      _Car_Wheels.RearRightMesh.transform.rotation = _wRotation_RR;
    }
    
    private void GoForward()
    {
      if(Mathf.Abs(_Local_Velocity_X) > 2.5f)
      {
        _Is_Drifting = true;
        CarDriftParticles();
      }
      else
      {
        _Is_Drifting = false;
        CarDriftParticles();
      }
      _Throttle_Axis += Time.deltaTime * 3f;
      if(_Throttle_Axis > 1f)
        _Throttle_Axis = 1f;

      if(_Local_Velocity_Z < -1f)
        Brakes();
      
      else{
        if(Mathf.RoundToInt(_Car_Speed) < _Characteristics.MaxSpeed){
          _Car_Wheels.FrontLeftCollider.brakeTorque = 0;
          _Car_Wheels.FrontLeftCollider.motorTorque = (_Characteristics.AccelerationMultiplier * 50f) * _Throttle_Axis;
          _Car_Wheels.FrontRightCollider.brakeTorque = 0;
          _Car_Wheels.FrontRightCollider.motorTorque = (_Characteristics.AccelerationMultiplier * 50f) * _Throttle_Axis;
          _Car_Wheels.RearLeftCollider.brakeTorque = 0;
          _Car_Wheels.RearLeftCollider.motorTorque = (_Characteristics.AccelerationMultiplier * 50f) * _Throttle_Axis;
          _Car_Wheels.RearRightCollider.brakeTorque = 0;
          _Car_Wheels.RearRightCollider.motorTorque = (_Characteristics.AccelerationMultiplier * 50f) * _Throttle_Axis;
        }
        else
          ThrottleOff();
      }
    }
    
    private void GoReverse()
    {
      if(Mathf.Abs(_Local_Velocity_X) > 2.5f)
      {
        _Is_Drifting = true;
        CarDriftParticles();
      }
      else
      {
        _Is_Drifting = false;
        CarDriftParticles();
      }

      _Throttle_Axis -= Time.deltaTime * 3f;
      if(_Throttle_Axis < -1f){
        _Throttle_Axis = -1f;
      }

      if(_Local_Velocity_Z > 1f)
        Brakes();
        
      else
      {
        if(Mathf.Abs(Mathf.RoundToInt(_Car_Speed)) < _Characteristics.MaxReverseSpeed)
        {
          _Car_Wheels.FrontLeftCollider.brakeTorque = 0;
          _Car_Wheels.FrontLeftCollider.motorTorque = _Characteristics.AccelerationMultiplier * 50f * _Throttle_Axis;
          
          _Car_Wheels.FrontRightCollider.brakeTorque = 0;
          _Car_Wheels.FrontRightCollider.motorTorque = _Characteristics.AccelerationMultiplier * 50f * _Throttle_Axis;
          
          _Car_Wheels.RearLeftCollider.brakeTorque = 0;
          _Car_Wheels.RearLeftCollider.motorTorque = _Characteristics.AccelerationMultiplier * 50f * _Throttle_Axis;
          
          _Car_Wheels.RearRightCollider.brakeTorque = 0;
          _Car_Wheels.RearRightCollider.motorTorque = _Characteristics.AccelerationMultiplier * 50f * _Throttle_Axis;
        }
        else
          ThrottleOff();
      }
    }
    
    private void ThrottleOff()
    {
      _Car_Wheels.FrontLeftCollider.motorTorque = 0;
      
      _Car_Wheels.FrontRightCollider.motorTorque = 0;
      
      _Car_Wheels.RearLeftCollider.motorTorque = 0;
      
      _Car_Wheels.RearRightCollider.motorTorque = 0;
    }
    
    private void DecelerateCar()
    {
      if(Mathf.Abs(_Local_Velocity_X) > 2.5f){
        _Is_Drifting = true;
        CarDriftParticles();
      }
      else
      {
        _Is_Drifting = false;
        CarDriftParticles();
      }

      if(_Throttle_Axis != 0f)
      {
        if(_Throttle_Axis > 0f)
          _Throttle_Axis -= Time.deltaTime * 10f;
        
        else if(_Throttle_Axis < 0f)
          _Throttle_Axis += Time.deltaTime * 10f;
        
        if(Mathf.Abs(_Throttle_Axis) < 0.15f)
          _Throttle_Axis = 0f;
      }
      _Rigidbody.velocity *=  1f / (1f + 0.025f * _Characteristics.DecelerationMultiplier);
      
      _Car_Wheels.FrontLeftCollider.motorTorque = 0;
      
      _Car_Wheels.FrontRightCollider.motorTorque = 0;
      
      _Car_Wheels.RearLeftCollider.motorTorque = 0;
      
      _Car_Wheels.RearRightCollider.motorTorque = 0;

      if(_Rigidbody.velocity.magnitude < 0.25f){
        _Rigidbody.velocity = Vector3.zero;
        CancelInvoke("DecelerateCar");
      }
    }
    
    private void Brakes()
    {
      _Car_Wheels.FrontLeftCollider.brakeTorque = _Characteristics.BrakeForce;
      
      _Car_Wheels.FrontRightCollider.brakeTorque = _Characteristics.BrakeForce;
      
      _Car_Wheels.RearLeftCollider.brakeTorque = _Characteristics.BrakeForce;
      
      _Car_Wheels.RearRightCollider.brakeTorque = _Characteristics.BrakeForce;
    }
    
    private void Handbrake()
    {
      CancelInvoke("RecoverTraction");

      _Drifting_Axis +=  (Time.deltaTime);
      float secureStartingPoint = _Drifting_Axis * _Front_Left_Wherl_ExtremumSlip * _Characteristics.DriftForce;

      if(secureStartingPoint < _Front_Left_Wherl_ExtremumSlip)
      {
        _Drifting_Axis = _Front_Left_Wherl_ExtremumSlip / (_Front_Left_Wherl_ExtremumSlip * _Characteristics.DriftForce);
      }
      if(_Drifting_Axis > 1f){
        _Drifting_Axis = 1f;
      }
 
      if(Mathf.Abs(_Local_Velocity_X) > 2.5f)
        _Is_Drifting = true;
      
      else
        _Is_Drifting = false;
      
      if(_Drifting_Axis < 1f)
      {
        _Front_Left_Wheel_Friction.extremumSlip = _Front_Left_Wherl_ExtremumSlip * _Characteristics.DriftForce * _Drifting_Axis;
        _Car_Wheels.FrontLeftCollider.sidewaysFriction = _Front_Left_Wheel_Friction;

        _Front_Right_Wheel_Friction.extremumSlip = _Front_Right_Wherl_ExtremumSlip * _Characteristics.DriftForce * _Drifting_Axis;
        _Car_Wheels.FrontRightCollider.sidewaysFriction = _Front_Right_Wheel_Friction;

        _Rear_Left_Wheel_Friction.extremumSlip = _Rear_Left_Wherl_ExtremumSlip * _Characteristics.DriftForce * _Drifting_Axis;
        _Car_Wheels.RearLeftCollider.sidewaysFriction = _Rear_Left_Wheel_Friction;

        _Rear_Right_Wheel_Friction.extremumSlip = _Rear_Right_Wherl_ExtremumSlip * _Characteristics.DriftForce * _Drifting_Axis;
        _Car_Wheels.RearRightCollider.sidewaysFriction = _Rear_Right_Wheel_Friction;
      }

      _Is_Traction_Locked = true;
      CarDriftParticles();

    }

    private void CarDriftParticles()
    {
        if(_Is_Drifting)
        {
            _RearLeft_Wheel_ParticleSystem.Play();
            _Rear_Right_Wheel_ParticleSystem.Play();
            PointsCountingSystem.OnDriftBeginning();
        }
        else
        {
            _RearLeft_Wheel_ParticleSystem.Stop();
            _Rear_Right_Wheel_ParticleSystem.Stop();
            PointsCountingSystem.OnDriftEnding();
        }

        if((_Is_Traction_Locked || Mathf.Abs(_Local_Velocity_X) > 5f) && Mathf.Abs(_Car_Speed) > 12f)
        {
            _Rear_Left_Wheel_Tire_Skid.emitting = true;
            _Rear_Right_Wheel_Tire_Skid.emitting = true;
        }
        else 
        {
            _Rear_Left_Wheel_Tire_Skid.emitting = false;
            _Rear_Right_Wheel_Tire_Skid.emitting = false;
        }
        
    }
    
    private void RecoverTraction()
    {
      _Is_Traction_Locked = false;
      _Drifting_Axis -= Time.deltaTime / 1.5f;
      
      if(_Drifting_Axis < 0f)
        _Drifting_Axis = 0f;
      
      if(_Front_Left_Wheel_Friction.extremumSlip > _Front_Left_Wherl_ExtremumSlip)
      {
        _Front_Left_Wheel_Friction.extremumSlip = _Front_Left_Wherl_ExtremumSlip * _Characteristics.DriftForce * _Drifting_Axis;
        _Car_Wheels.FrontLeftCollider.sidewaysFriction = _Front_Left_Wheel_Friction;

        _Front_Right_Wheel_Friction.extremumSlip = _Front_Right_Wherl_ExtremumSlip * _Characteristics.DriftForce * _Drifting_Axis;
        _Car_Wheels.FrontRightCollider.sidewaysFriction = _Front_Right_Wheel_Friction;

        _Rear_Left_Wheel_Friction.extremumSlip = _Rear_Left_Wherl_ExtremumSlip * _Characteristics.DriftForce * _Drifting_Axis;
        _Car_Wheels.RearLeftCollider.sidewaysFriction = _Rear_Left_Wheel_Friction;

        _Rear_Right_Wheel_Friction.extremumSlip = _Rear_Right_Wherl_ExtremumSlip * _Characteristics.DriftForce * _Drifting_Axis;
        _Car_Wheels.RearRightCollider.sidewaysFriction = _Rear_Right_Wheel_Friction;

        Invoke("RecoverTraction", Time.deltaTime);

      }else if (_Front_Left_Wheel_Friction.extremumSlip < _Front_Left_Wherl_ExtremumSlip)
      {
        _Front_Left_Wheel_Friction.extremumSlip = _Front_Left_Wherl_ExtremumSlip;
        _Car_Wheels.FrontLeftCollider.sidewaysFriction = _Front_Left_Wheel_Friction;

        _Front_Right_Wheel_Friction.extremumSlip = _Front_Right_Wherl_ExtremumSlip;
        _Car_Wheels.FrontRightCollider.sidewaysFriction = _Front_Right_Wheel_Friction;

        _Rear_Left_Wheel_Friction.extremumSlip = _Rear_Left_Wherl_ExtremumSlip;
        _Car_Wheels.RearLeftCollider.sidewaysFriction = _Rear_Left_Wheel_Friction;

        _Rear_Right_Wheel_Friction.extremumSlip = _Rear_Right_Wherl_ExtremumSlip;
        _Car_Wheels.RearRightCollider.sidewaysFriction = _Rear_Right_Wheel_Friction;

        _Drifting_Axis = 0f;
      }
    }
    
    private void OnDisable() => GameEvents._On_Level_Ended -= DisableMovement;
}

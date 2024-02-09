/*
MESSAGE FROM CREATOR: This script was coded by Mena. You can use it in your games either these are commercial or
personal projects. You can even add or remove functions as you wish. However, you cannot sell copies of this
script by itself, since it is originally distributed as a free product.
I wish you the best for your project. Good luck!

P.S: If you need more cars, you can check my other vehicle assets on the Unity Asset Store, perhaps you could find
something useful for your game. Best regards, Mena.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{
      private FixedJoystick _Joystick;
      private DriftButton _Drift_Button;

      private GameUI _Game_UI;
  
      [SerializeField] private int _Max_Speed = 90;
      [SerializeField] private int _Max_Reverse_Speed = 45;
      [SerializeField] private int _Acceleration_Multiplier = 2;
      [SerializeField] private int _Max_Steering_Angle = 27;

      [SerializeField] private float _Steering_Speed = 0.5f;
      
      [SerializeField] private int _Brake_Force = 350;
      [SerializeField] private int _Deceleration_Multiplier = 2;
      [SerializeField] private int _Handbrake_Drift_Multiplier = 5;
      
      [SerializeField] private Vector3 _Body_Mass_Center; 
                                    
      [SerializeField] private GameObject _Front_Left_Mesh;
      [SerializeField] private WheelCollider _Front_Left_Collider;

      [SerializeField] private GameObject _Front_Right_Mesh;
      [SerializeField] private WheelCollider _Front_Right_Collider;

      [SerializeField] private GameObject _Rear_Left_Mesh;
      [SerializeField] private WheelCollider _Rear_Left_Collider;

      [SerializeField] private GameObject _Rear_Right_Mesh;
      [SerializeField] private WheelCollider _Rear_Right_Collider;
      
      [SerializeField] private bool _Use_Effects = false;
      
      [SerializeField] private ParticleSystem _RearLeft_Wheel_ParticleSystem;
      [SerializeField] private ParticleSystem _Rear_Right_Wheel_ParticleSystem;
      
      [SerializeField] private TrailRenderer _Rear_Left_Wheel_Tire_Skid;
      [SerializeField] private TrailRenderer _Rear_Right_Wheel_Tire_Skid;

      private float _Car_Speed;
      
      private bool _Is_Drifting;
      private bool _Is_Traction_Locked;


      private Rigidbody _Rigidbody;
      
      private float _Steering_Axis; 
      private float _Throttle_Axis; 
      private float _Drifting_Axis;
      private float _Local_Velocity_Z;
      private float _Local_Velocity_X;
      
      private bool _Decelerating_Car;

      
      WheelFrictionCurve _Front_Left_Wheel_Friction;
      float _Front_Left_Wherl_ExtremumSlip;
      WheelFrictionCurve _Front_Right_Wheel_Friction;
      float _Front_Right_Wherl_ExtremumSlip;
      WheelFrictionCurve _Rear_Left_Wheel_Friction;
      float _Rear_Left_Wherl_ExtremumSlip;
      WheelFrictionCurve _Rear_Right_Wheel_Friction;
      float _Rear_Right_Wherl_ExtremumSlip;
      
      public float MaxSpeed{get=>_Max_Speed;}
      public float DriftForce{get=>_Handbrake_Drift_Multiplier;}

      private PhotonView _Photon_View;

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
      
      _Rigidbody.centerOfMass = _Body_Mass_Center;

      WheelsSetup();
      
      CheckEffects();

    }
    
    private void FixedUpdate()
    {
      if (SceneMediator.IsPhoton && !_Photon_View.IsMine)
        return;
      
      _Car_Speed = (2 * Mathf.PI * _Front_Left_Collider.radius * _Front_Left_Collider.rpm * 60) / 1000;

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

    private void WheelsSetup()
    {
      _Front_Left_Wheel_Friction = new WheelFrictionCurve ();
        _Front_Left_Wheel_Friction.extremumSlip = _Front_Left_Collider.sidewaysFriction.extremumSlip;
        _Front_Left_Wherl_ExtremumSlip = _Front_Left_Collider.sidewaysFriction.extremumSlip;
        _Front_Left_Wheel_Friction.extremumValue = _Front_Left_Collider.sidewaysFriction.extremumValue;
        _Front_Left_Wheel_Friction.asymptoteSlip = _Front_Left_Collider.sidewaysFriction.asymptoteSlip;
        _Front_Left_Wheel_Friction.asymptoteValue = _Front_Left_Collider.sidewaysFriction.asymptoteValue;
        _Front_Left_Wheel_Friction.stiffness = _Front_Left_Collider.sidewaysFriction.stiffness;

        _Front_Right_Wheel_Friction.extremumSlip = _Front_Right_Collider.sidewaysFriction.extremumSlip;
        _Front_Right_Wherl_ExtremumSlip = _Front_Right_Collider.sidewaysFriction.extremumSlip;
        _Front_Right_Wheel_Friction.extremumValue = _Front_Right_Collider.sidewaysFriction.extremumValue;
        _Front_Right_Wheel_Friction.asymptoteSlip = _Front_Right_Collider.sidewaysFriction.asymptoteSlip;
        _Front_Right_Wheel_Friction.asymptoteValue = _Front_Right_Collider.sidewaysFriction.asymptoteValue;
        _Front_Right_Wheel_Friction.stiffness = _Front_Right_Collider.sidewaysFriction.stiffness;
        
      _Rear_Left_Wheel_Friction = new WheelFrictionCurve ();
        _Rear_Left_Wheel_Friction.extremumSlip = _Rear_Left_Collider.sidewaysFriction.extremumSlip;
        _Rear_Left_Wherl_ExtremumSlip = _Rear_Left_Collider.sidewaysFriction.extremumSlip;
        _Rear_Left_Wheel_Friction.extremumValue = _Rear_Left_Collider.sidewaysFriction.extremumValue;
        _Rear_Left_Wheel_Friction.asymptoteSlip = _Rear_Left_Collider.sidewaysFriction.asymptoteSlip;
        _Rear_Left_Wheel_Friction.asymptoteValue = _Rear_Left_Collider.sidewaysFriction.asymptoteValue;
        _Rear_Left_Wheel_Friction.stiffness = _Rear_Left_Collider.sidewaysFriction.stiffness;
        
      _Rear_Right_Wheel_Friction = new WheelFrictionCurve ();
        _Rear_Right_Wheel_Friction.extremumSlip = _Rear_Right_Collider.sidewaysFriction.extremumSlip;
        _Rear_Right_Wherl_ExtremumSlip = _Rear_Right_Collider.sidewaysFriction.extremumSlip;
        _Rear_Right_Wheel_Friction.extremumValue = _Rear_Right_Collider.sidewaysFriction.extremumValue;
        _Rear_Right_Wheel_Friction.asymptoteSlip = _Rear_Right_Collider.sidewaysFriction.asymptoteSlip;
        _Rear_Right_Wheel_Friction.asymptoteValue = _Rear_Right_Collider.sidewaysFriction.asymptoteValue;
        _Rear_Right_Wheel_Friction.stiffness = _Rear_Right_Collider.sidewaysFriction.stiffness;
    }

    private void InputSystem()
    {
      
      if(_Joystick.Vertical > 0){
        CancelInvoke("DecelerateCar");
        _Decelerating_Car = false;
        GoForward();
      }
      
      if(_Joystick.Vertical < 0){
        CancelInvoke("DecelerateCar");
        _Decelerating_Car = false;
        GoReverse();
      }
      
      if(_Joystick.Horizontal < 0){
        TurnLeft();
      }
      
      if(_Joystick.Horizontal > 0){
        TurnRight();
      }
      
      if(_Drift_Button.IsTouched){
        CancelInvoke("DecelerateCar");
        _Decelerating_Car = false;
        Handbrake();
      }
      
      if(!_Drift_Button.IsTouched){
        RecoverTraction();
      }
      
      if(_Joystick.Vertical == 0){
        ThrottleOff();
      }
      
      if(_Joystick.Vertical == 0  && !_Drift_Button.IsTouched && !_Decelerating_Car){
        InvokeRepeating("DecelerateCar", 0f, 0.1f);
        _Decelerating_Car = true;
      }
      
      if(_Joystick.Horizontal == 0 && _Steering_Axis != 0f){
        ResetSteeringAngle();

      }
    }

    private void CheckEffects()
    {
      if(!_Use_Effects){
        if(_RearLeft_Wheel_ParticleSystem != null){
          _RearLeft_Wheel_ParticleSystem.Stop();
        }
        if(_Rear_Right_Wheel_ParticleSystem != null){
          _Rear_Right_Wheel_ParticleSystem.Stop();
        }
        if(_Rear_Left_Wheel_Tire_Skid != null){
          _Rear_Left_Wheel_Tire_Skid.emitting = false;
        }
        if(_Rear_Right_Wheel_Tire_Skid != null){
          _Rear_Right_Wheel_Tire_Skid.emitting = false;
        }
      }
    }

    private void DisableMovement()
    {
      _Max_Speed = 0;
      _Steering_Speed = 0;

      enabled = false;
    }
    
    private void TurnLeft(){
      _Steering_Axis = _Steering_Axis - (Time.deltaTime * 10f * _Steering_Speed);
      if(_Steering_Axis < -1f){
        _Steering_Axis = -1f;
      }
      var steeringAngle = _Steering_Axis * _Max_Steering_Angle;
      _Front_Left_Collider.steerAngle = Mathf.Lerp(_Front_Left_Collider.steerAngle, steeringAngle, _Steering_Speed);
      _Front_Right_Collider.steerAngle = Mathf.Lerp(_Front_Right_Collider.steerAngle, steeringAngle, _Steering_Speed);
    }

    private void TurnRight(){
      _Steering_Axis = _Steering_Axis + (Time.deltaTime * 10f * _Steering_Speed);
      if(_Steering_Axis > 1f){
        _Steering_Axis = 1f;
      }
      var steeringAngle = _Steering_Axis * _Max_Steering_Angle;
      _Front_Left_Collider.steerAngle = Mathf.Lerp(_Front_Left_Collider.steerAngle, steeringAngle, _Steering_Speed);
      _Front_Right_Collider.steerAngle = Mathf.Lerp(_Front_Right_Collider.steerAngle, steeringAngle, _Steering_Speed);
    }
    
    private void ResetSteeringAngle(){
      if(_Steering_Axis < 0f){
        _Steering_Axis = _Steering_Axis + (Time.deltaTime * 10f * _Steering_Speed);
      }else if(_Steering_Axis > 0f){
        _Steering_Axis = _Steering_Axis - (Time.deltaTime * 10f * _Steering_Speed);
      }
      if(Mathf.Abs(_Front_Left_Collider.steerAngle) < 1f){
        _Steering_Axis = 0f;
      }
      var steeringAngle = _Steering_Axis * _Max_Steering_Angle;
      _Front_Left_Collider.steerAngle = Mathf.Lerp(_Front_Left_Collider.steerAngle, steeringAngle, _Steering_Speed);
      _Front_Right_Collider.steerAngle = Mathf.Lerp(_Front_Right_Collider.steerAngle, steeringAngle, _Steering_Speed);
    }
    
    private void AnimateWheelMeshes(){
      try{
        Quaternion FLWRotation;
        Vector3 FLWPosition;
        _Front_Left_Collider.GetWorldPose(out FLWPosition, out FLWRotation);
        _Front_Left_Mesh.transform.position = FLWPosition;
        _Front_Left_Mesh.transform.rotation = FLWRotation;

        Quaternion FRWRotation;
        Vector3 FRWPosition;
        _Front_Right_Collider.GetWorldPose(out FRWPosition, out FRWRotation);
        _Front_Right_Mesh.transform.position = FRWPosition;
        _Front_Right_Mesh.transform.rotation = FRWRotation;

        Quaternion RLWRotation;
        Vector3 RLWPosition;
        _Rear_Left_Collider.GetWorldPose(out RLWPosition, out RLWRotation);
        _Rear_Left_Mesh.transform.position = RLWPosition;
        _Rear_Left_Mesh.transform.rotation = RLWRotation;

        Quaternion RRWRotation;
        Vector3 RRWPosition;
        _Rear_Right_Collider.GetWorldPose(out RRWPosition, out RRWRotation);
        _Rear_Right_Mesh.transform.position = RRWPosition;
        _Rear_Right_Mesh.transform.rotation = RRWRotation;
      }catch(Exception ex){
        Debug.LogWarning(ex);
      }
    }
    
    private void GoForward(){

      if(Mathf.Abs(_Local_Velocity_X) > 2.5f){
        _Is_Drifting = true;
        DriftCarPS();
      }else{
        _Is_Drifting = false;
        DriftCarPS();
      }
      _Throttle_Axis = _Throttle_Axis + (Time.deltaTime * 3f);
      if(_Throttle_Axis > 1f){
        _Throttle_Axis = 1f;
      }

      if(_Local_Velocity_Z < -1f){
        Brakes();
      }else{
        if(Mathf.RoundToInt(_Car_Speed) < _Max_Speed){
          _Front_Left_Collider.brakeTorque = 0;
          _Front_Left_Collider.motorTorque = (_Acceleration_Multiplier * 50f) * _Throttle_Axis;
          _Front_Right_Collider.brakeTorque = 0;
          _Front_Right_Collider.motorTorque = (_Acceleration_Multiplier * 50f) * _Throttle_Axis;
          _Rear_Left_Collider.brakeTorque = 0;
          _Rear_Left_Collider.motorTorque = (_Acceleration_Multiplier * 50f) * _Throttle_Axis;
          _Rear_Right_Collider.brakeTorque = 0;
          _Rear_Right_Collider.motorTorque = (_Acceleration_Multiplier * 50f) * _Throttle_Axis;
        }
        else 
        {
          _Front_Left_Collider.motorTorque = 0;
    			_Front_Right_Collider.motorTorque = 0;
          _Rear_Left_Collider.motorTorque = 0;
    			_Rear_Right_Collider.motorTorque = 0;
    		}
      }
    }
    
    private void GoReverse()
    {
      if(Mathf.Abs(_Local_Velocity_X) > 2.5f)
      {
        _Is_Drifting = true;
        DriftCarPS();
      }
      else
      {
        _Is_Drifting = false;
        DriftCarPS();
      }

      _Throttle_Axis = _Throttle_Axis - (Time.deltaTime * 3f);
      if(_Throttle_Axis < -1f){
        _Throttle_Axis = -1f;
      }

      if(_Local_Velocity_Z > 1f)
      {
        Brakes();
        
      }
      else
      {
        if(Mathf.Abs(Mathf.RoundToInt(_Car_Speed)) < _Max_Reverse_Speed)
        {
          _Front_Left_Collider.brakeTorque = 0;
          _Front_Left_Collider.motorTorque = (_Acceleration_Multiplier * 50f) * _Throttle_Axis;
          _Front_Right_Collider.brakeTorque = 0;
          _Front_Right_Collider.motorTorque = (_Acceleration_Multiplier * 50f) * _Throttle_Axis;
          _Rear_Left_Collider.brakeTorque = 0;
          _Rear_Left_Collider.motorTorque = (_Acceleration_Multiplier * 50f) * _Throttle_Axis;
          _Rear_Right_Collider.brakeTorque = 0;
          _Rear_Right_Collider.motorTorque = (_Acceleration_Multiplier * 50f) * _Throttle_Axis;
        }
        else 
        {
          _Front_Left_Collider.motorTorque = 0;
    			_Front_Right_Collider.motorTorque = 0;
          _Rear_Left_Collider.motorTorque = 0;
    			_Rear_Right_Collider.motorTorque = 0;
    		}
      }
    }
    
    private void ThrottleOff()
    {
      _Front_Left_Collider.motorTorque = 0;
      _Front_Right_Collider.motorTorque = 0;
      _Rear_Left_Collider.motorTorque = 0;
      _Rear_Right_Collider.motorTorque = 0;
    }
    
    private void DecelerateCar()
    {
      if(Mathf.Abs(_Local_Velocity_X) > 2.5f){
        _Is_Drifting = true;
        DriftCarPS();
      }
      else
      {
        _Is_Drifting = false;
        DriftCarPS();
      }

      if(_Throttle_Axis != 0f)
      {
        if(_Throttle_Axis > 0f)
        {
          _Throttle_Axis = _Throttle_Axis - (Time.deltaTime * 10f);
        }
        else if(_Throttle_Axis < 0f)
        {
            _Throttle_Axis = _Throttle_Axis + (Time.deltaTime * 10f);
        }
        if(Mathf.Abs(_Throttle_Axis) < 0.15f)
        {
          _Throttle_Axis = 0f;
        }
      }
      _Rigidbody.velocity = _Rigidbody.velocity * (1f / (1f + (0.025f * _Deceleration_Multiplier)));
      _Front_Left_Collider.motorTorque = 0;
      _Front_Right_Collider.motorTorque = 0;
      _Rear_Left_Collider.motorTorque = 0;
      _Rear_Right_Collider.motorTorque = 0;

      if(_Rigidbody.velocity.magnitude < 0.25f){
        _Rigidbody.velocity = Vector3.zero;
        CancelInvoke("DecelerateCar");
      }
    }
    
    private void Brakes()
    {
      _Front_Left_Collider.brakeTorque = _Brake_Force;
      _Front_Right_Collider.brakeTorque = _Brake_Force;
      _Rear_Left_Collider.brakeTorque = _Brake_Force;
      _Rear_Right_Collider.brakeTorque = _Brake_Force;
    }
    
    private void Handbrake()
    {
      CancelInvoke("RecoverTraction");

      _Drifting_Axis = _Drifting_Axis + (Time.deltaTime);
      float secureStartingPoint = _Drifting_Axis * _Front_Left_Wherl_ExtremumSlip * _Handbrake_Drift_Multiplier;

      if(secureStartingPoint < _Front_Left_Wherl_ExtremumSlip)
      {
        _Drifting_Axis = _Front_Left_Wherl_ExtremumSlip / (_Front_Left_Wherl_ExtremumSlip * _Handbrake_Drift_Multiplier);
      }
      if(_Drifting_Axis > 1f){
        _Drifting_Axis = 1f;
      }
 
      if(Mathf.Abs(_Local_Velocity_X) > 2.5f)
      {
        _Is_Drifting = true;
      }
      else
      {
        _Is_Drifting = false;
      }
 
      if(_Drifting_Axis < 1f)
      {
        _Front_Left_Wheel_Friction.extremumSlip = _Front_Left_Wherl_ExtremumSlip * _Handbrake_Drift_Multiplier * _Drifting_Axis;
        _Front_Left_Collider.sidewaysFriction = _Front_Left_Wheel_Friction;

        _Front_Right_Wheel_Friction.extremumSlip = _Front_Right_Wherl_ExtremumSlip * _Handbrake_Drift_Multiplier * _Drifting_Axis;
        _Front_Right_Collider.sidewaysFriction = _Front_Right_Wheel_Friction;

        _Rear_Left_Wheel_Friction.extremumSlip = _Rear_Left_Wherl_ExtremumSlip * _Handbrake_Drift_Multiplier * _Drifting_Axis;
        _Rear_Left_Collider.sidewaysFriction = _Rear_Left_Wheel_Friction;

        _Rear_Right_Wheel_Friction.extremumSlip = _Rear_Right_Wherl_ExtremumSlip * _Handbrake_Drift_Multiplier * _Drifting_Axis;
        _Rear_Right_Collider.sidewaysFriction = _Rear_Right_Wheel_Friction;
      }

      _Is_Traction_Locked = true;
      DriftCarPS();

    }

    private void DriftCarPS()
    {

      if(_Use_Effects){
        if(_Is_Drifting)
          {
            _RearLeft_Wheel_ParticleSystem.Play();
            _Rear_Right_Wheel_ParticleSystem.Play();
            PointsCountingSystem.OnDriftBeginning();
          }
          else if(!_Is_Drifting)
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
      else if(!_Use_Effects)
      {
        if(_RearLeft_Wheel_ParticleSystem != null)
        {
          _RearLeft_Wheel_ParticleSystem.Stop();
        }
        if(_Rear_Right_Wheel_ParticleSystem != null)
        {
          _Rear_Right_Wheel_ParticleSystem.Stop();
        }
        if(_Rear_Left_Wheel_Tire_Skid != null)
        {
          _Rear_Left_Wheel_Tire_Skid.emitting = false;
        }
        if(_Rear_Right_Wheel_Tire_Skid != null)
        {
          _Rear_Right_Wheel_Tire_Skid.emitting = false;
        }
      }

    }
    
    private void RecoverTraction()
    {
      _Is_Traction_Locked = false;
      _Drifting_Axis = _Drifting_Axis - (Time.deltaTime / 1.5f);
      if(_Drifting_Axis < 0f){
        _Drifting_Axis = 0f;
      }
      
      if(_Front_Left_Wheel_Friction.extremumSlip > _Front_Left_Wherl_ExtremumSlip)
      {
        _Front_Left_Wheel_Friction.extremumSlip = _Front_Left_Wherl_ExtremumSlip * _Handbrake_Drift_Multiplier * _Drifting_Axis;
        _Front_Left_Collider.sidewaysFriction = _Front_Left_Wheel_Friction;

        _Front_Right_Wheel_Friction.extremumSlip = _Front_Right_Wherl_ExtremumSlip * _Handbrake_Drift_Multiplier * _Drifting_Axis;
        _Front_Right_Collider.sidewaysFriction = _Front_Right_Wheel_Friction;

        _Rear_Left_Wheel_Friction.extremumSlip = _Rear_Left_Wherl_ExtremumSlip * _Handbrake_Drift_Multiplier * _Drifting_Axis;
        _Rear_Left_Collider.sidewaysFriction = _Rear_Left_Wheel_Friction;

        _Rear_Right_Wheel_Friction.extremumSlip = _Rear_Right_Wherl_ExtremumSlip * _Handbrake_Drift_Multiplier * _Drifting_Axis;
        _Rear_Right_Collider.sidewaysFriction = _Rear_Right_Wheel_Friction;

        Invoke("RecoverTraction", Time.deltaTime);

      }else if (_Front_Left_Wheel_Friction.extremumSlip < _Front_Left_Wherl_ExtremumSlip)
      {
        _Front_Left_Wheel_Friction.extremumSlip = _Front_Left_Wherl_ExtremumSlip;
        _Front_Left_Collider.sidewaysFriction = _Front_Left_Wheel_Friction;

        _Front_Right_Wheel_Friction.extremumSlip = _Front_Right_Wherl_ExtremumSlip;
        _Front_Right_Collider.sidewaysFriction = _Front_Right_Wheel_Friction;

        _Rear_Left_Wheel_Friction.extremumSlip = _Rear_Left_Wherl_ExtremumSlip;
        _Rear_Left_Collider.sidewaysFriction = _Rear_Left_Wheel_Friction;

        _Rear_Right_Wheel_Friction.extremumSlip = _Rear_Right_Wherl_ExtremumSlip;
        _Rear_Right_Collider.sidewaysFriction = _Rear_Right_Wheel_Friction;

        _Drifting_Axis = 0f;
      }
    }
    
    private void OnDisable()
    {
      GameEvents._On_Level_Ended -= DisableMovement;
    }

}

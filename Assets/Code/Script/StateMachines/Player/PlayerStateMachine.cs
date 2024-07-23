using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
  [field: SerializeField] public InputReader InputReader { get; private set; }
  [field: SerializeField] public CharacterController Controller { get; private set; }
  [field: SerializeField] public Animator Animator { get; private set; }
  
  [field: Header("Movement")]
  [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
  [field: SerializeField] public float RunningMovementSpeed { get; private set; }
  [field: SerializeField] public float RotationDamping { get; private set; }
  [field: SerializeField] public float JumpForce { get; private set; }
  [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
  [field: SerializeField] public bool IsRunning {get; set;}
  
  [field: Header("Equipment")]
  [field: SerializeField] public Transform weaponEquipPosition;
  [field: SerializeField] public Transform weaponUnequipPosition;
  [field: SerializeField] public bool isWeaponEquip { get; set; }
  
  public Transform MainCameraTransform { get; private set; }
  
  void Start()
  {
    MainCameraTransform = Camera.main.transform;
    
    SwitchState(new PlayerFreeLookState(this));
  }

  void Awake() {
    InputReader.RunningEvent += HandleRunning;
  }

  void OnDestroy() {
      InputReader.RunningEvent -= HandleRunning;
  }

  private void HandleRunning() {
      IsRunning = !IsRunning;
  }
}

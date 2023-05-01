using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using TreeEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;
using UnityEngine.InputSystem.LowLevel;

public class ForkliftPlayer : MonoBehaviour
{
    Vector2 moveInput;
    public bool isBreaking;
    [Header("Player Movement")]
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private Transform respawnPoint;
    public float currentBreakForce;
    public SoundManager soundManager;
    private float currrentSteeringAngle;

    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider backLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private Transform frontLeftT;
    [SerializeField] private Transform backLeftT;
    [SerializeField] private Transform frontRightT;
    [SerializeField] private Transform backRightT;
    [SerializeField] private Transform centerOfMass;

    [Header("Lift")]
    [SerializeField] private GameObject Lift;
    [SerializeField] private Transform LiftBottom;
    [SerializeField] private Transform LiftTop;
    [SerializeField] private float liftSpeed = 0.2f;
    bool liftUp;
    bool liftDown;

    [Header("HUD")]
    [SerializeField] private Animator HUDAnimator;
    private float curLiftSpeed;
    private bool lastLiftUp = false;
    public float liftPercent;



    private bool playedReverseSound = false;
    private bool playedBrakeSound = false;
    private bool playedLiftSound = false;

    private void Awake()
    {
        Lift.transform.position = LiftTop.position;
        Physics.IgnoreLayerCollision(10, 11);
        GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Cursor.lockState = CursorLockMode.Locked;
        else if (Input.GetKeyDown(KeyCode.Mouse1)) Cursor.lockState = CursorLockMode.None;
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        MoveLift();
        PlaySounds(); 

    }

    private void PlaySounds()
    {
        if (frontLeft.motorTorque < 0f)
        {
            if (!playedReverseSound)
            {
                playedReverseSound = true;
                soundManager.OnReverse.Invoke();
            }
        }
        else
        {
            if (playedReverseSound)
            {
                playedReverseSound = false;
                soundManager.OnReverseStop.Invoke();
            }
        }

        if (currentBreakForce > 0f && GetComponent<Rigidbody>().velocity.magnitude > 1f)
        {
            Debug.Log("Breaking");
            if (!playedBrakeSound)
            {
                playedBrakeSound = true;
                soundManager.OnBrake.Invoke();
            }
        }
        else
        {
            if (playedBrakeSound)
            {
                playedBrakeSound = false;
                soundManager.OnBrakeStop.Invoke();
            }
        }
        if(liftDown || liftUp)
        {
            if (liftPercent == 1 || liftPercent == 0)
            {
                if (playedLiftSound)
                {
                    soundManager.OnForkliftStop.Invoke();
                    playedLiftSound = false;
                }
            }
            else
            {
                if (!playedLiftSound)
                {
                    soundManager.OnForklift.Invoke();
                    playedLiftSound = true;
                }
            }
        }
        else
        {
            if (playedLiftSound)
            {
                soundManager.OnForkliftStop.Invoke();
                playedLiftSound = false;
            }
        }

    }

    private void HandleMotor()
    {
        frontLeft.motorTorque = moveInput.y * motorForce;
        frontRight.motorTorque = moveInput.y * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;

        


        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontLeft.brakeTorque = currentBreakForce;
        frontRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currrentSteeringAngle = maxSteeringAngle * moveInput.x;
        frontLeft.steerAngle = currrentSteeringAngle;
        frontRight.steerAngle = currrentSteeringAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeft, frontLeftT);
        UpdateSingleWheel(frontRight, frontRightT);
        UpdateSingleWheel(backLeft, backLeftT);
        UpdateSingleWheel(backRight, backRightT);
    }

    private void UpdateSingleWheel(WheelCollider wheel, Transform wheelTrans)
    {
        Vector3 pos;
        Quaternion rot;
        wheel.GetWorldPose(out pos, out rot);
        wheelTrans.position = pos;
        wheelTrans.rotation = rot;
    }

    private void MoveLift()
    {
        if (liftUp)
        {
            if (!lastLiftUp)
            {
                liftPercent = 1 - liftPercent;
                lastLiftUp = true;
            }


            liftPercent += Time.deltaTime * liftSpeed;
            Lift.transform.position = Vector3.Lerp(LiftBottom.position, LiftTop.position, liftPercent);
        }

        if (liftDown)
        {
            if (lastLiftUp)
            {
                liftPercent = 1 - liftPercent;
                lastLiftUp = false;
            }

            liftPercent += Time.deltaTime * liftSpeed;
            Lift.transform.position = Vector3.Lerp(LiftTop.position, LiftBottom.position, liftPercent);

        }
        liftPercent = Mathf.Clamp(liftPercent, 0, 1);

    }

    #region Inputs
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnBreakingPress(InputValue value)
    {
        isBreaking = true;
    }

    private void OnBreakingRelease(InputValue value)
    {
        isBreaking = false;
    }

    private void OnLiftUpPress(InputValue value)
    {
        liftUp = true;
    }

    private void OnLiftUpRelease(InputValue value)
    {
        liftUp = false;
    }

    private void OnLiftDownPress(InputValue value)
    {
        liftDown = true;
    }

    private void OnLiftDownRelease(InputValue value)
    {
        liftDown = false;
    }

    private void OnReset(InputValue value)
    {
        gameObject.transform.position = respawnPoint.position;
        gameObject.transform.rotation = respawnPoint.rotation;
    }

    private void OnSeeDeliveryPress(InputValue value)
    {
        HUDAnimator.SetBool("ShowNote", true);
    }
    private void OnSeeDeliveryRelease(InputValue value)
    {
        HUDAnimator.SetBool("ShowNote", false);
    }
    #endregion
}

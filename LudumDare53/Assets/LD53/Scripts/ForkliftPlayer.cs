using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class ForkliftPlayer : MonoBehaviour
{
    Vector2 moveInput;
    public bool isBreaking;
    [Header("Player Movement")]
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;
    public float currentBreakForce;
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

    private float curLiftSpeed;
    private bool lastLiftUp = false;
    public float liftPercent;

    private void Awake()
    {
        Lift.transform.position = LiftTop.position;
        Physics.IgnoreLayerCollision(10, 11);
        GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, Vector3.up.y, gameObject.transform.position.z);
            gameObject.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        MoveLift();
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
    #endregion
}

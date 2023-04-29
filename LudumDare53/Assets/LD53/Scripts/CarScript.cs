using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarScript : MonoBehaviour
{
    Vector2 moveInput;
    bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;
    private float currentBreakForce;
    private float currrentSteeringAngle;

    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider backLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private Transform frontLeftT;
    [SerializeField] private Transform backLeftT;
    [SerializeField] private Transform frontRightT;
    [SerializeField] private Transform backRightT;

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        //UpdateWheels();
    }

    private void HandleMotor()
    {
        frontLeft.motorTorque = moveInput.y * motorForce;
        frontRight.motorTorque = moveInput.y * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        if (isBreaking)
        {
            ApplyBreaking();
        }
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
        UpdateSingleWheel(frontLeft);
        UpdateSingleWheel(frontRight);
        UpdateSingleWheel(backLeft);
        UpdateSingleWheel(backRight);
    }

    private void UpdateSingleWheel(WheelCollider wheel)
    {

    }

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
        isBreaking = true;
    }
}

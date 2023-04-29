using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    [Header("Lift")]
    [SerializeField] private GameObject Lift;
    [SerializeField] private Transform LiftBottom;
    [SerializeField] private Transform LiftTop;
    [SerializeField] private float liftSpeed = 0.2f;

    [Header("Player")]
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private float rotationSpeed = 40f;

    bool liftUp;
    bool liftDown;

    private float curLiftSpeed;
    private bool lastLiftUp = true;
    public float liftPercent;
    private Vector2 playerInput;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Lift.transform.position = LiftBottom.position;
        Physics.IgnoreLayerCollision(10, 11);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Cursor.lockState = CursorLockMode.Locked;
        else if (Input.GetKeyDown(KeyCode.Mouse1)) Cursor.lockState = CursorLockMode.None; 
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * playerInput.y * playerSpeed * Time.deltaTime, ForceMode.Impulse);
        //transform.Rotate(transform.up, rotationSpeed * playerInput.x * Time.deltaTime);
        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * (rotationSpeed * playerInput.x * Time.deltaTime));
        rb.MoveRotation(transform.rotation * deltaRotation);

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

    #region InputActions
    private void OnMove(InputValue value)
    {
        playerInput = value.Get<Vector2>();
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

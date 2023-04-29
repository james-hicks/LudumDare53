using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private GameObject Lift;
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private float rotationSpeed = 40f;

    private Vector2 playerInput;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Cursor.lockState = CursorLockMode.Locked;
        else if (Input.GetKeyDown(KeyCode.Mouse1)) Cursor.lockState = CursorLockMode.None; 
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * playerInput.y * playerSpeed * Time.deltaTime, ForceMode.Impulse);
        transform.Rotate(transform.up, rotationSpeed * playerInput.x * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        playerInput = value.Get<Vector2>();
    }


}

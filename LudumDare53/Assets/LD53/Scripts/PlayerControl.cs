using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject Lift;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Cursor.lockState = CursorLockMode.Locked;
        else if (Input.GetKeyDown(KeyCode.Mouse1)) Cursor.lockState = CursorLockMode.None;


        if(Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Raise Lift");
        }
        
        if (Input.GetKey(KeyCode.E)) 
        {
            Debug.Log("Lower Lift");
        }
    }
}

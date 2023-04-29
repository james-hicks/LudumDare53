using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TruckCollider : MonoBehaviour
{
    private GameManager gameManager;
    public List<GameObject> collectedBoxes = new List<GameObject>();
    public bool PlayerInTruck;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if(gameManager.currentOrder.orderComplete && !PlayerInTruck)
        {
            Leave();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Box")
        {
            collectedBoxes.Add(other.gameObject);
            gameManager.CheckCollectedBoxes(collectedBoxes);
        }
        
        if(other.tag == "Player")
        {
            PlayerInTruck = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Box")
        {
            collectedBoxes.Remove(other.gameObject);
            gameManager.CheckCollectedBoxes(collectedBoxes);
        }
        
        if (other.tag == "Player")
        {
            PlayerInTruck = false;
        }
    }

    public void Leave()
    {

    }

    public void EndOrder()
    {
        gameManager.EndOrder();
    }
}

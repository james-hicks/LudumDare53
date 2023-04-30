using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TruckCollider : MonoBehaviour
{
    private GameManager gameManager;
    public List<GameObject> collectedBoxes = new List<GameObject>();
    public bool PlayerInTruck;
    [SerializeField] private Animator truckAnimator;
    private bool left = false;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        
    }

    private void Start()
    {
        StartCoroutine(CheckOrderDone());
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

    private IEnumerator CheckOrderDone()
    {
        while(true)
        {
            if (gameManager.currentOrder.orderComplete)
            {
                yield return new WaitForSeconds(2f);
                if(!PlayerInTruck && gameManager.currentOrder.orderComplete)
                {
                    if (!left)
                    {
                        left = true;
                        Leave();
                    }

                }
            }
            yield return null;
        }
    }

    public void Leave()
    {
       
        truckAnimator.SetTrigger("ResetTruck");
    }

    public void EndOrder()
    {
        gameManager.EndOrder();
    }

    public void StartNewLoad()
    {
        gameManager.CreateOrder();
    }
}

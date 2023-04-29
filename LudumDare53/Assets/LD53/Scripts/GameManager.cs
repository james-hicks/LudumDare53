using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Range(0, 10)][SerializeField] private int minItems;
    [Range(0, 10)][SerializeField] private int maxItems;
    [SerializeField] private GameObject[] boxes;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] usedSpawnPoints;
    public Order currentOrder;

    [Header("Debug")]
    public List<GameObject> spawnedBoxes = new List<GameObject>();
    public int boxesCollected;

    private void Awake()
    {
        CreateOrder();
    }

    private void Update()
    {
        
    }

    public void CreateOrder()
    {
        Debug.Log("Creating Order");
        Order newOrder = new Order();

        int orderLength = Random.Range(minItems, maxItems);
        Debug.Log("Order Length =" + orderLength);
        for(int i = 0; i < orderLength; i++)
        {
            newOrder.OrderRequirements.Add(boxes[Random.Range(0, boxes.Length)]);
        }
        newOrder.reqBoxes = newOrder.OrderRequirements.Count;
        currentOrder = newOrder;
       StartCoroutine(StartNewOrder());
    }


    private IEnumerator StartNewOrder()
    {
        Debug.Log("Starting Order");
        for (int i = 0; i < currentOrder.OrderRequirements.Count; i++)
        {
            
            GameObject spawnPoint;
            bool goodSpawnPoint;
            // Verify Spawn Point
            do
            {
                goodSpawnPoint = true;
                
                Debug.Log("Attempting SpawnPoints");
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                for (int b = 0; b < usedSpawnPoints.Length; b++)
                {
                    if (usedSpawnPoints[b] == spawnPoint)
                    {
                        goodSpawnPoint = false;
                        Debug.Log("Bad Spawnpoint");
                        break;
                    }
                }
                yield return new WaitForEndOfFrame();
            } while (!goodSpawnPoint);

            for (int b = 0; b < usedSpawnPoints.Length; b++)
            {
                if (usedSpawnPoints[b] == null)
                {
                    usedSpawnPoints[b] = spawnPoint;
                    break;
                }
            }

            spawnedBoxes.Add(Instantiate(currentOrder.OrderRequirements[i], spawnPoint.transform.position, Quaternion.identity));
        }
    }

    public void EndOrder()
    {
        Debug.Log("Finished Order");
        for (int i = 0; i < usedSpawnPoints.Length; i++)
        {
            usedSpawnPoints[i] = null;
        }

        for (int a = 0; a < spawnedBoxes.Count; a++)
        {
            Destroy(spawnedBoxes[a]);
        }
        spawnedBoxes.Clear();
        CreateOrder();
    }

    public void CheckCollectedBoxes(List<GameObject> collectedBoxes)
    {
        boxesCollected = 0;
        for (int a = 0; a < collectedBoxes.Count; a++)
        {
            for (int i = 0; i < spawnedBoxes.Count; i++)
            {
                if (collectedBoxes[a] == spawnedBoxes[i])
                {
                    Debug.Log("Matching Box");
                    boxesCollected++;
                }
            }
        }

        if(boxesCollected == currentOrder.reqBoxes)
        {
            currentOrder.orderComplete = true;
            Debug.Log("Order Complete");
        }
    }
}



public class Order
{
    public List<GameObject> OrderRequirements = new List<GameObject>();
    public int reqBoxes;
    public bool orderComplete;

}

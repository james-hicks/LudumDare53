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
        for(int i = 0; i < orderLength; i++)
        {

            newOrder.OrderRequirements.Add(boxes[Random.Range(0, boxes.Length)]);
        }
        currentOrder = newOrder;
        StartNewOrder();
    }

    private void StartNewOrder()
    {
        for(int i = 0; i < currentOrder.OrderRequirements.Count; i++)
        {
            Debug.Log("Starting Order");
            bool goodSpawnPoint = true;
            GameObject spawnPoint;

            // Verify Spawn Point
            do
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                for(int b = 0; b< usedSpawnPoints.Length; b++)
                {
                    if (usedSpawnPoints[i] == spawnPoint)
                    {
                        goodSpawnPoint = false;
                        break;
                    }
                }
            } while (!goodSpawnPoint);

            for (int b = 0; b < usedSpawnPoints.Length; b++)
            {
                if (usedSpawnPoints[b] == null)
                {
                    usedSpawnPoints[b] = spawnPoint;
                }
            }

            GameObject spawnedBox = Instantiate(currentOrder.OrderRequirements[i], spawnPoint.transform.position, Quaternion.identity);
        }
    }

    private void EndOrder()
    {
        Debug.Log("Finished Order");
        for (int i = 0; i < usedSpawnPoints.Length; i++)
        {
            usedSpawnPoints[i] = null;
        }

        CreateOrder();
    }
}

public class Order
{
    public List<GameObject> OrderRequirements;
    public bool metRequirements = false;

}

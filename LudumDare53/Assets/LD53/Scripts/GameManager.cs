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

    private void Awake()
    {
        CreateOrder();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EndOrder();
        }
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
        currentOrder = newOrder;
        StartNewOrder();
    }

    private void StartNewOrder()
    {
        Debug.Log("Starting Order");
        for (int i = 0; i < currentOrder.OrderRequirements.Count; i++)
        {
            bool goodSpawnPoint = true;
            GameObject spawnPoint;

            // Verify Spawn Point
            do
            {
                Debug.Log("Attempting SpawnPoints");
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                 
                for(int b = 0; b < usedSpawnPoints.Length; b++)
                {
                    if (usedSpawnPoints[b] == spawnPoint)
                    {
                        goodSpawnPoint = false;
                        Debug.Log("Bad Spawnpoint");
                        break;
                    }
                }
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

    private void EndOrder()
    {
        Debug.Log("Finished Order");
        for (int i = 0; i < usedSpawnPoints.Length; i++)
        {
            usedSpawnPoints[i] = null;
        }

        //for (int a = 0; a< spawnedBoxes.Count; a++)
        //{
        //    Destroy(spawnedBoxes[a]);
        //}
        //spawnedBoxes.Clear();

        //CreateOrder();
    }
}

public class Order
{
    public List<GameObject> OrderRequirements = new List<GameObject>();
    public bool metRequirements = false;

}

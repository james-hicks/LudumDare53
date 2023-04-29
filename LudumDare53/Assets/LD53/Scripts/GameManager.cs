using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Range(0, 10)][SerializeField] private int minItems;
    [Range(0, 10)][SerializeField] private int maxItems;
    [SerializeField] private GameObject[] boxes;
    public Order currentOrder;

    private void Awake()
    {
        CreateOrder();
    }

    public void CreateOrder()
    {
        Order newOrder = new Order();
        int orderLength = Random.Range(minItems, maxItems);
        for(int i = 0; i < orderLength; i++)
        {
            Debug.Log($"Box {i+1}/{orderLength}");
        }

        currentOrder = newOrder;
    }
}

public class Order
{
    public List<GameObject> OrderRequirements;

}

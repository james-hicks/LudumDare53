using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAnimEvents : MonoBehaviour
{
    [SerializeField] private TruckCollider truckCollider;
    public void EndLoad()
    {
        truckCollider.EndOrder();
    }

    public void SpawnNewLoad()
    {
        truckCollider.StartNewLoad();
    }
}

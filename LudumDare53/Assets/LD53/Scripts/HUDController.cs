using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private TextMeshProUGUI smallAmount;
    [SerializeField] private TextMeshProUGUI tallAmount;
    [SerializeField] private TextMeshProUGUI mediumAmount;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        tallAmount.text = gameManager.boxTypes[2].ToString();
        smallAmount.text = gameManager.boxTypes[0].ToString();
        mediumAmount.text = gameManager.boxTypes[1].ToString();
    }
}

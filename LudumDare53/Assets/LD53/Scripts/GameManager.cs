using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    [Range(0, 10)][SerializeField] private int maxItems;
    [SerializeField] private GameObject[] boxes;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] usedSpawnPoints;
    public Order currentOrder;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI truckText;
    private SoundManager soundManager;
    private MenuManager menuManager;
    private bool timerActive = false;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private TextMeshProUGUI GameOverText;

    [Header("Debug")]
    public List<GameObject> spawnedBoxes = new List<GameObject>();
    public int boxesCollected;
    public int[] boxTypes;
    private int ordersCompleted = 0;
    public float timer;

    private bool playedSuccessSound;
    private bool playedFailSound;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        soundManager = FindObjectOfType<SoundManager>();
        menuManager = GetComponent<MenuManager>();
        CreateOrder();
    }

    private void Update()
    {
        if (timerActive)
        {
            timer -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(timer);
            if (timer <= 0)
            {
                if (!playedFailSound)
                {
                    menuManager.GameOver = true;
                    Debug.Log("GAME OVER");
                    GameOver();
                    playedFailSound = true;
                    soundManager.FailOrder.Invoke();
                }
            }
            else
            {
                if (timerText != null) timerText.text = time.ToString(@"m\:ss");
            }
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        GameOverText.text = "You filled " + ordersCompleted.ToString() + " orders before you were fired. . .";
        GameOverScreen.SetActive(true);
    }

    public void CreateOrder()
    {
        Order newOrder = new Order();
        int orderLength;
        for (int i = 0; i < boxTypes.Length; i++)
        {
            boxTypes[i] = 0;
        }

        if (ordersCompleted == 0)
        {
            orderLength = 1;
        }else if (ordersCompleted <= 2)
        {
            orderLength = Random.Range(2,4);
        }
        else
        {
            orderLength = Random.Range(2, maxItems + 1);
        }


        Debug.Log("Order Length =" + orderLength);

        for(int i = 0; i < orderLength; i++)
        {
            int newBox = Random.Range(0, boxes.Length);

            if(newBox <= 3)
            {
                boxTypes[0]++;
            }
            else if(newBox <= 6) 
            {
                boxTypes[1]++;
            }
            else
            {
                boxTypes[2]++;
            }
            newOrder.OrderRequirements.Add(boxes[newBox]);
        }

        newOrder.reqBoxes = newOrder.OrderRequirements.Count;
        currentOrder = newOrder;
       StartCoroutine(StartNewOrder());
    }


    private IEnumerator StartNewOrder()
    {
        playedSuccessSound = false;
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

            if (ordersCompleted <= 4)
            {
                timer = 300f; // 5 mins
                
            } else if(ordersCompleted <= 6)
            {
                timer = 240f; // 4 mins
            } else
            {
                timer = 180f; // 3 mins
            }


            timerActive = true;

            spawnedBoxes.Add(Instantiate(currentOrder.OrderRequirements[i], spawnPoint.transform.position, Quaternion.identity));
        }
    }

    public void EndOrder()
    {
        Debug.Log("Finished Order");
        timerActive = false;
        for (int i = 0; i < usedSpawnPoints.Length; i++)
        {
            usedSpawnPoints[i] = null;
        }

        for (int a = 0; a < spawnedBoxes.Count; a++)
        {
            Destroy(spawnedBoxes[a]);
        }
        spawnedBoxes.Clear();
        if (!playedSuccessSound)
        {
            playedSuccessSound = true;
            soundManager.CompleteOrder.Invoke();
        }
        ordersCompleted++;
        if(truckText != null) truckText.text = ordersCompleted.ToString();
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
                    boxesCollected++;
                }
            }
        }

        if(boxesCollected == currentOrder.reqBoxes)
        {
            currentOrder.orderComplete = true;
            Debug.Log("Order Delivered");
        }
        else
        {
            currentOrder.orderComplete = false;
        }
    }
}



public class Order
{
    public List<GameObject> OrderRequirements = new List<GameObject>();
    public int reqBoxes;
    public bool orderComplete;

}

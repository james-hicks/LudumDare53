using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HamsterNPC : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolPointReachedDistance = 3f;
    private int _patrolIndex;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(PatrolState());
    }
    private IEnumerator PatrolState()
    {
        Transform patrolPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];

        while (true)
        {
            float patrolDistance = Vector3.Distance(transform.position, patrolPoint.position);

            if (patrolDistance < patrolPointReachedDistance) patrolPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];


            agent.SetDestination(patrolPoint.position);
            Debug.DrawLine(transform.position, patrolPoint.position, Color.blue);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Hit a Hamster!");
        }
    }
}

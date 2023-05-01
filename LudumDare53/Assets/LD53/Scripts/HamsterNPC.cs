using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HamsterNPC : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolPointReachedDistance = 3f;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource source;
    private int _patrolIndex;
    private IEnumerator currentState;

    public bool KnockedDown;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(PatrolState());
    }

    private void ChangeState(IEnumerator newState)
    {
        if(currentState != null) StopCoroutine(currentState);

        currentState = newState;
        StartCoroutine(currentState);
    }

    private IEnumerator PatrolState()
    {
        Transform patrolPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];

        while (true)
        {
            float patrolDistance = Vector3.Distance(transform.position, patrolPoint.position);

            if (patrolDistance < patrolPointReachedDistance) patrolPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];

            if(KnockedDown)
            {
                ChangeState(GetUpState());
            }
            agent.SetDestination(patrolPoint.position);
            Debug.DrawLine(transform.position, patrolPoint.position, Color.blue);
            yield return null;
        }
    }

    private IEnumerator GetUpState()
    {
        animator.SetTrigger("Hit");
        source.Play();
        while (KnockedDown)
        {
            agent.isStopped = true;
            yield return null;
        }
        agent.isStopped = false;
        ChangeState(PatrolState());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !KnockedDown)
        {
            KnockedDown = true;
        }
    }

    public void UnKnockDown()
    {
        KnockedDown = false;
    }
}

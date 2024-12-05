using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Lapin : Animal
{
    public NavMeshAgent agent;
    public float wanderTimer;

    private NavMeshTester food;
    private Transform target;
    private float timer;
    // Start is called before the first frame update
    void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    private void OnValidate()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= wanderTimer)
            {
                Wandering();
            }
        else if (food != null)
            {
                GoingToFoodSource();
            }

    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Set the target if a valid one enters in the detection range
        food = other.GetComponent<NavMeshTester>();
    }
    private void OnTriggerExit(Collider other)
    {
        // Forget the food if it exits the detection range
        if (other == food)
        {
            food = null;
        }
    }
    private void Wandering()
    {
        Vector3 newPos = RandomNavSphere(transform.position, sensoryDistance, -1);
        agent.SetDestination(newPos);
        timer = 0;
    }
    private void GoingToFoodSource()
    {
        if (hunger >= 20)
        {
            agent.SetDestination(food.transform.position);
        }
    }
}

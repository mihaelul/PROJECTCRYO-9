using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
   
    public float speed = 3f;
    public float wanderRadius = 10f;
    public float changeDirectionInterval = 3f;

    private Vector3 targetPosition;
    private float timer;

    void Start()
    {
        PickNewTarget();
        timer = changeDirectionInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

     
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);

    
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f || timer <= 0f)
        {
            PickNewTarget();
            timer = changeDirectionInterval;
        }
    }

    void PickNewTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection.y = 0; 

        targetPosition = transform.position + randomDirection;
    }

}
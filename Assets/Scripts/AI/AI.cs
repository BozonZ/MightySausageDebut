using System;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour, IDamageable
{
    [SerializeField] protected AIData data = null;
    public AIData GetData => data;
    protected NavMeshAgent agent;
    public Transform target;
    public GameObject stream;
    protected float health;

    public Action OnDeathEvent;
    public event Action<AI> ReturnToPoolEvent;

    private void Start() {
        data = Instantiate(data);
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = data.Acceleration;
        agent.speed = data.MoveSpeed;
        agent.angularSpeed = data.RotateSpeed;
        agent.stoppingDistance = 5;
        health = data.Health;
        stream.SetActive(false);
    }

    public void Spawn(Vector3 start, Transform target) {
        if (start == null || target == null) {
            Debug.LogWarning("Invalid start/target given!");
            return;
        }
        transform.position = start;
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        this.target = target;
        //agent.SetDestination(target.position);
    }

    public void Update() {
        if (target == null)
            return;

        agent.SetDestination(target.position);
        agent.stoppingDistance = 5;
        Attack();
    }

    public void OnDamaged() {
        health -= Time.deltaTime;
        if(health <= 0)
            OnDeath();
    }

    public void OnDeath() {
        target = null;
        health = data.Health;
        OnDeathEvent?.Invoke();
        if (ReturnToPoolEvent == null)
            Debug.LogWarning("No methods attached to returntopoolEvent!");
        else
            ReturnToPoolEvent.Invoke(this);
    }

    private enum AIState
    {
        Attack 
    }

    public void Attack()
    {
        if (target == null)
            return;

        agent.transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

        if (agent.velocity.sqrMagnitude == 0f)
        {
            stream.SetActive(true);
        }
        else
        {
            stream.SetActive(false);
        }
    }
    
}

using System;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour, IDamageable
{
    [SerializeField] private AIData data = null;
    public AIData GetData => data;
    private NavMeshAgent agent;
    public event Action<AI> ReturnToPoolEvent;
    public Action OnDeathEvent;
    private Transform target;
    private int health;

    private void Start() {
        data = Instantiate(data);
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = data.acceleration;
        agent.speed = data.moveSpeed;
        agent.angularSpeed = data.rotateSpeed;
        health = data.health;
    }

    private void Update() {
        if (target == null)
            return;
        agent.SetDestination(target.position);
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
        agent.SetDestination(target.position);
    }

    public void OnDamaged() {
        health--;
        if(health <= 0)
            OnDeath();
    }

    public void OnDeath() {
        target = null;
        health = data.health;
        OnDeathEvent?.Invoke();
        if (ReturnToPoolEvent == null)
            Debug.LogWarning("No methods attached to returntopoolEvent!");
        else
            ReturnToPoolEvent.Invoke(this);
    }
}

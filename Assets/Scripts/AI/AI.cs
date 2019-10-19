using System;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour, IDamageable
{
    [SerializeField] protected AIData data = null;
    public AIData GetData => data;
    protected NavMeshAgent agent;
    protected Transform target;
    protected float health;

    public Action OnDeathEvent;
    public event Action<AI> ReturnToPoolEvent;

    private void Start() {
        data = Instantiate(data);
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = data.Acceleration;
        agent.speed = data.MoveSpeed;
        agent.angularSpeed = data.RotateSpeed;
        agent.stoppingDistance = data.Range;
        health = data.Health;
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

    public void Attack() {
        if (target == null)
            return;

        agent.SetDestination(target.position);
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

    private enum AIState {
        Attack
    }
}

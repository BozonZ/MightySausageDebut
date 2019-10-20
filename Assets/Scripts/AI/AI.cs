#pragma warning disable CS0649
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour, IDamageable {
    [SerializeField] protected AIData data = null;
    public AIData GetData => data;
    protected NavMeshAgent agent;
    public Transform target;
    protected float health;

    public Action OnDeathEvent;
    public event Action<AI> ReturnToPoolEvent;
    [SerializeField] private AudioClip[] attackAudios;
    [SerializeField] private float attackRate;
    [SerializeField] private LayerMask sightMask;
    [SerializeField] private float attackFade;
    [SerializeField] private Vector3 attackOffset;
    [SerializeField] private Vector3 attackSize;
    [SerializeField] private LayerMask attackMask;
    private LineRenderer lineRenderer;
    private Coroutine attackCoroutine;
    private Coroutine attackFadeCoroutine;
    private AudioSource source;

    private void Start() {
        data = Instantiate(data);
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = data.Acceleration;
        agent.speed = data.MoveSpeed;
        agent.angularSpeed = data.RotateSpeed;
        agent.stoppingDistance = data.Range;
        health = data.Health;

        //Attack
        lineRenderer = GetComponent<LineRenderer>();

        //sound
        source = GetComponent<AudioSource>();
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
    }

    public void Update() {
        if (target == null)
            return;

        agent.SetDestination(target.position);
        Attack();
    }

    public void OnDamaged(float damage) {
        health = 0f;
        if (health <= 0f)
            OnDeath();
    }

    public void OnDeath() {
        //Renderer
        lineRenderer.enabled = false;
        lineRenderer.material.SetColor("_Color", lineRenderer.startColor);
        attackFadeCoroutine = null;

        target = null;
        health = data.Health;
        OnDeathEvent?.Invoke();
        if (ReturnToPoolEvent == null)
            Debug.LogWarning("No methods attached to returntopoolEvent!");
        else
            ReturnToPoolEvent.Invoke(this);
    }

    public void Attack() {
        if (target == null)
            return;

        agent.transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        if (agent.velocity.sqrMagnitude == 0f && (agent.transform.position - target.position).magnitude < data.Range) {
            if (attackFadeCoroutine != null) {
                StopCoroutine(attackFadeCoroutine);
            }
            lineRenderer.enabled = true;
            RaycastHit hit;
            if (!Physics.Raycast(transform.position + attackOffset, transform.forward, out hit, data.Range, sightMask)) {
                if (Physics.BoxCast(transform.position + attackOffset, attackSize, transform.forward, out hit, Quaternion.identity, data.Range, attackMask)) {
                    Debug.Log("Attack");
                    lineRenderer.SetPosition(0, transform.position + attackOffset);
                    lineRenderer.SetPosition(1, hit.point);
                    if (!source.isPlaying) {
                        source.PlayOneShot(attackAudios[UnityEngine.Random.Range(0, attackAudios.Length)]);
                    }
                    if (attackCoroutine == null) {
                        attackCoroutine = StartCoroutine(AttackCoroutine(hit.collider));
                    }
                }
            }
        } else {
            if (attackFadeCoroutine == null)
                attackFadeCoroutine = StartCoroutine(FadeAttack());
        }
    }

    private IEnumerator AttackCoroutine(Collider hit) {
        hit.GetComponent<IDamageable>()?.OnDamaged(data.Damage);
        yield return new WaitForSeconds(attackRate);
        attackCoroutine = null;
    }

    private IEnumerator FadeAttack() {

        float tempFade = 0f;
        Color color = lineRenderer.startColor;
        while (tempFade < attackFade) {
            tempFade += Time.deltaTime;
            color = Color.Lerp(color, new Color(0, 0f, 0f, 0f), Time.deltaTime / attackFade);
            lineRenderer.material.SetColor("_Color", color);
            yield return null;
        }

        lineRenderer.enabled = false;
        color = lineRenderer.startColor;
        lineRenderer.material.SetColor("_Color", color);
        attackFadeCoroutine = null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + attackOffset, attackSize);
    }

}

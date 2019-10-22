#pragma warning disable CS0649
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator animator;
    [SerializeField] private float toIdle;
    [SerializeField] private float maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private float turnRate;
    [SerializeField] private AudioClip[] walkAudios;
    private Vector3 velocity;
    private Vector3 rotation;
    private Rigidbody rb;
    private AudioSource audioSource;
    [SerializeField] private float health;
    private float tempIdle;
    private bool isDeath;
    public GameObject pauseMenu;

    void Start()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("Pause");
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        Move();
        
    }

    private void Move()
    {

        velocity.x = speed * Input.GetAxisRaw("Horizontal");
        velocity.z = speed * Input.GetAxisRaw("Vertical");
        //Sound
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(walkAudios[UnityEngine.Random.Range(0, walkAudios.Length)]);
            animator.SetBool("Walk", true);
            animator.SetBool("Idle", false);
            tempIdle = toIdle;
        }
        else
        {
            animator.SetBool("Walk", false);
            if (tempIdle > 0f)
            {
                tempIdle -= Time.deltaTime;
                if (tempIdle <= 0f)
                {
                    animator.SetBool("Idle", true);
                }
            }
        }
        rb.velocity += velocity;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
        rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(rotation, Vector3.up), turnRate * Time.deltaTime);
        rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(rotation, Vector3.up), turnRate);
        rotation = velocity.magnitude > 0.01f || velocity.magnitude < -0.01f ? velocity : rotation;
        velocity = Vector3.zero;
    }

    public void AddHealth(float healAmount)
    {
        health += healAmount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void OnDamaged(float damage)
    {
        if (isDeath)
            return;

        health -= damage * 2;
        Debug.Log($"{name} has {health} left");
        animator.SetBool("Idle", false);
        animator.SetTrigger("Damaged");
        if (health <= 0f)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        isDeath = true;
        animator.SetBool("Death", true);
        StartCoroutine(Dead());
        pauseMenu.SetActive(true);
    }

    public IEnumerator Dead()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        Time.timeScale = 0;
    }
    
}
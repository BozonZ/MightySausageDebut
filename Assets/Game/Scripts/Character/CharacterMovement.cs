#pragma warning disable CS0649
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour, IDamageable {
    [SerializeField] private float maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private float turnRate;
    [SerializeField] private AudioClip[] walkAudios;
    private Vector3 velocity;
    private Vector3 rotation;
    private Rigidbody rb;
    private AudioSource audioSource;
    private float health;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {

        velocity.x = speed * Input.GetAxisRaw("Horizontal");
        velocity.z = speed * Input.GetAxisRaw("Vertical");
        //Sound
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f) {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(walkAudios[Random.Range(0, walkAudios.Length)], 0.3f);
        }

        rb.velocity += velocity;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
        rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(rotation, Vector3.up), turnRate * Time.deltaTime);
        rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(rotation, Vector3.up), turnRate);
        rotation = velocity.magnitude > 0.01f || velocity.magnitude < -0.01f ? velocity : rotation;
        velocity = Vector3.zero;
    }

    public void OnDamaged(float damage) {
        health -= damage;
        Debug.Log($"{name} has {health} left");
        if (health <= 0f) {
            OnDeath();
        }
    }

    public void OnDeath() {
        Debug.Log($"{name} is dead!");
    }

}
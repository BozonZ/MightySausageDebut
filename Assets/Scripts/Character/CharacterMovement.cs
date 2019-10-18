
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float turnRate;
    private Vector3 velocity;
    private Vector3 rotation;
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {

    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {

        velocity.x = speed * Input.GetAxisRaw("Horizontal");
        velocity.z = speed * Input.GetAxisRaw("Vertical");
        rb.velocity += velocity;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
        rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(rotation, Vector3.up), turnRate * Time.deltaTime);
        rotation = velocity.magnitude > 0.01f || velocity.magnitude < -0.01f ? velocity : rotation;
        velocity = Vector3.zero;
    }
}

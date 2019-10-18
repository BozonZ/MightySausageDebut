 
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 velocity;
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
        velocity = Vector3.zero;
    }
}

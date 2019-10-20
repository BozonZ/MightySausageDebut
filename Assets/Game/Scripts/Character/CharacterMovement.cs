
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour {
    [SerializeField]
    private float speed;
    [SerializeField]
    private float turnRate;
    private Vector3 velocity;
    private Vector3 rotation;
    private Rigidbody rb;
    public float health = 100;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        Move();
        Health();

        Mathf.Clamp(health, -5, 100);
    }

    private void Move()
    {

        velocity.x = speed * Input.GetAxisRaw("Horizontal");
        velocity.z = speed * Input.GetAxisRaw("Vertical");
        rb.velocity += velocity;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
        rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(rotation, Vector3.up), turnRate);
        rotation = velocity.magnitude > 0.01f || velocity.magnitude < -0.01f ? velocity : rotation;
        velocity = Vector3.zero;
    }

    public void Health()
    {
        if(health < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Water")
        {
            health -= 5;
        }

        if (collision.transform.tag == "Health")
        {
            health += 25;
        }
    }
}

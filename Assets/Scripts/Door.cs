using UnityEngine;
[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour {

    [SerializeField] private bool isLocked = false;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other) {
        if (isLocked)
            return;

        animator.SetBool("state", true);
    }

    private void OnTriggerExit(Collider other) {
        if (isLocked)
            return;

        animator.SetBool("state", false);
    }
}

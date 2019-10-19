using UnityEngine;

public abstract class Receiver : MonoBehaviour
{
    [SerializeField] protected Trigger trigger;

    private void Start() {
        trigger.OnTrigger += Activate;
    }

    protected abstract void Activate();
}

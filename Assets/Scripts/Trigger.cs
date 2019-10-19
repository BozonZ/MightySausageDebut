using System;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    public event Action OnTrigger;

    private void OnTriggerStay(Collider other) {
        HasTrigger();
    }

    protected virtual void HasTrigger() {
        InvokeTriggerEvent();
    }

    private void OnTriggerExit(Collider other) {
        ExitTrigger();
    }

    protected abstract void ExitTrigger();

    protected void InvokeTriggerEvent() {
        OnTrigger.Invoke();
    }
}

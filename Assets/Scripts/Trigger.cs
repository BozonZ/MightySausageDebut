using System;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public event Action OnTrigger;

    private void OnTriggerEnter(Collider other) {
        OnTrigger.Invoke();
    }
}

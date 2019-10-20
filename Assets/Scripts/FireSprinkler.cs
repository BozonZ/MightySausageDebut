#pragma warning disable CS0649
using System.Collections;
using UnityEngine;
using JoelQ.Helper;
public class FireSprinkler : Receiver {

    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float sprinklerDamage;
    [SerializeField] private float damageInterval;
    [SerializeField] private LayerMask sprinklerMask;
    private AudioSource audioSource;
    private Coroutine damageCoroutine;

    protected override void Start() {
        base.Start();
        if (!particle) {
            particle = GetComponent<ParticleSystem>();
        }
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Activate() {
        audioSource.Play();

        if (particle.isPlaying) {
            audioSource.Stop();
            particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        } else {
            particle.Play(true);
        }
    }

    protected void OnTriggerStay(Collider other) {
        if (other.gameObject.layer.CompareLayer(sprinklerMask)) {
            if (damageCoroutine == null) {
                damageCoroutine = StartCoroutine(SprinklerDamage(other));
            }
        }
    }

    private IEnumerator SprinklerDamage(Collider other) {
        other.GetComponent<CharacterMovement>()?.AddHealth(-sprinklerDamage);
        yield return new WaitForSeconds(damageInterval);
        damageCoroutine = null;
    }
}

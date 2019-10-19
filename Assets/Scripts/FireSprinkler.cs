#pragma warning disable CS0649
using UnityEngine;
public class FireSprinkler : Receiver {

    [SerializeField] private ParticleSystem particle;
    private AudioSource audioSource;
    protected override void Start() {
        base.Start();
        if(!particle) {
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
}

using UnityEngine;
public class FireSprinkler : Receiver {

    [SerializeField] private ParticleSystem particle;

    private void Start() {
        if(!particle) {
            particle = GetComponent<ParticleSystem>();
        }
    }

    protected override void Activate() {

        if (particle.isPlaying) {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        } else {
            particle.Play(true);
        }
    }
}

#pragma warning disable CS0649
using System.Collections;
using UnityEngine;

public class FireAlarmTrigger : Trigger
{
    [SerializeField] private float timeToTrigger;
    [SerializeField] private float timeTillDisable;
    [SerializeField] private AudioClip sensorAudio;
    [SerializeField] private AudioClip alarmAudio;
    private AudioSource audioSource;

    protected void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sensorAudio;
    }

    protected override void HasTrigger() {

        if (timeToTrigger > 0f) {
            timeToTrigger -= Time.deltaTime;
            if(!audioSource.isPlaying) {
                audioSource.Play();
            }
            if (timeToTrigger < 0f) {
                audioSource.clip = alarmAudio;
                audioSource.Play();
                StartCoroutine(DisableTrigger());
                InvokeTriggerEvent();
            }
        }
    }

    protected override void ExitTrigger() {
        if(audioSource.clip == sensorAudio)
            audioSource.Stop();
    }

    private IEnumerator DisableTrigger() {
        yield return new WaitForSeconds(timeTillDisable);
        InvokeTriggerEvent();
        audioSource.Stop();
    }
}

#pragma warning disable CS0649
using System.Collections;
using UnityEngine;
using JoelQ.Helper;

public class CharacterAttack : MonoBehaviour {
    [SerializeField] private Vector3 laserOffset;
    [SerializeField] private Vector3 laserSize;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCD;
    [SerializeField] private LayerMask attackMask;
    [SerializeField] private AudioClip laserAudio;
    [SerializeField] private AudioClip[] absorbAudios;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float fadeTime;
    private Coroutine attackCoroutine;
    private AudioSource audioSource;

    private void Start() {
        if (!lineRenderer) {
            Debug.Log("Attack line renderer is missing!");
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && attackCoroutine == null) {
            audioSource.PlayOneShot(laserAudio);
            StartCoroutine(AttackRenderer(transform.position + laserOffset, (transform.position + laserOffset) + (transform.forward * attackRange)));
            attackCoroutine = StartCoroutine(TimedAttack());
        }
    }

    private IEnumerator TimedAttack() {
        Attack();
        yield return new WaitForSeconds(attackCD);
        attackCoroutine = null;
    }

    private void Attack() {
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(transform.position + laserOffset, laserSize / 2, transform.forward, Quaternion.identity, attackRange, attackMask);
        foreach (RaycastHit hit in hits) {
            IDamageable iDamage = hit.collider.GetComponent<IDamageable>();
            iDamage?.OnDeath();
        }
    }

    private IEnumerator AttackRenderer(Vector3 startPos, Vector3 endPos) {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        float tempFade = 0f;
        Color color = lineRenderer.startColor;
        while (tempFade < fadeTime) {
            tempFade += Time.deltaTime;
            color = Color.Lerp(color, new Color(0f, 0f, 0f, 0f), Time.deltaTime / fadeTime);
            lineRenderer.material.SetColor("_Color", color);
            yield return null;
        }

        lineRenderer.enabled = false;
        color = lineRenderer.startColor;
        lineRenderer.material.SetColor("_Color", color);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer.CompareLayer(attackMask)) {
            IDamageable iDamage = other.collider.GetComponent<IDamageable>();
            audioSource.PlayOneShot(absorbAudios[Random.Range(0, absorbAudios.Length)], 0.5f);
            iDamage?.OnDeath();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position + laserOffset, laserSize);
    }
}
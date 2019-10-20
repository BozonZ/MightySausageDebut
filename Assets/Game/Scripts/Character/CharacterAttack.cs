using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private Coroutine attackCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && attackCoroutine == null)
        {
            Attack();
            attackCoroutine = StartCoroutine(TimedAttack());
        }
    }
    IEnumerator TimedAttack()
    {
        yield return new WaitForSeconds(5);
        attackCoroutine = null;
    }

    public void Attack()
    {
        Debug.DrawRay(transform.position, Vector3.forward, Color.red);
        if (Physics.Raycast(transform.position, Vector3.forward, 10))
        {

        }
    }
}
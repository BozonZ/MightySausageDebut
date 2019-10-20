using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    public Animator stream;

    void Start()
    {
        stream = GetComponent<Animator>();
        stream.Play("Entry");
    }
    
}

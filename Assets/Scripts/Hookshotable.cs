using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshotable : MonoBehaviour
{
    public Rigidbody hookedRb;
    // Start is called before the first frame update
    void Awake()
    {
        if(hookedRb == null)
        {
            hookedRb = GetComponent<Rigidbody>();
            if (hookedRb == null) hookedRb = gameObject.GetComponentInChildren<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

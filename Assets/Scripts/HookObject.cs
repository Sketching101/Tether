using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookObject : MonoBehaviour
{
    public SwingMechanics HookShot;

    public bool testFire = false, testDrop = false;
    public bool inGun = true;

    public Transform StartFrom;

    [SerializeField] private float LaunchForce;

    public Rigidbody attachedTo = null;
    private Rigidbody hookRb = null;

    void Awake()
    {
        hookRb = GetComponent<Rigidbody>();
        hookRb.isKinematic = true;
        hookRb.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hookRb.AddForce(Physics.gravity * -.5f, ForceMode.Acceleration);
        if (inGun)
            transform.position = StartFrom.position;
    }

    public void LaunchHook(Vector3 Direction)
    {
        hookRb.isKinematic = false;
        hookRb.useGravity = true;

        inGun = false;
        testFire = false;
        transform.position = StartFrom.transform.position;

        hookRb.AddForce(Direction * LaunchForce);
    }

    public void DetachHook()
    {
        inGun = true;
        hookRb.transform.parent = null;
        hookRb.isKinematic = true;
        hookRb.useGravity = false;
        attachedTo = null;
        HookShot.DetachTether();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Hookshotable>() != null && !inGun)
        {
            Debug.Log("Hooked!");
            hookRb.isKinematic = true;
            hookRb.useGravity = false;
            gameObject.transform.parent = other.transform;
            attachedTo = other.gameObject.GetComponent<Hookshotable>().hookedRb;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherLauncher : MonoBehaviour
{
    public SwingMechanics HookShot;

    [SerializeField] private Transform GrappleGun;
    [SerializeField] private HookObject Hook;

    private Transform HookAnchor;
    void Awake()
    {
        HookAnchor = Hook.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hook.attachedTo != null)
            HookShot.SetTethered();
    }
}

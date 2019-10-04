using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimHookshot : MonoBehaviour
{
    [Header("Aim Reticles")]
    /// <summary>
    /// The reticle that appears on the screen
    /// and the reticle that the hook will actually fire at
    /// </summary>
    [SerializeField] private RectTransform AimReticleReal;

    [Header("Other Important Anchors")]
    [SerializeField] private Camera MainCamera = null;
    [SerializeField] private Transform HookShotGun;

    private Vector3 PlayerToTarget;

    void Awake()
    {
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRealReticle();
    }

     void FixedUpdate()
    {
        
    }


    private void UpdateRealReticle()
    {
        // Layer 8 is defined as "Hookshotable" so we want layerMask to represent only that layer
        int layerMask = 1 << 8;
        RaycastHit hit;
        Ray VirtualRay = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(VirtualRay, out hit, 200.0f, layerMask))
        {
            Vector3 TargetWorldPosition = hit.point;
            PlayerToTarget = TargetWorldPosition - HookShotGun.position;

            Vector3 newPosition = MainCamera.WorldToScreenPoint(TargetWorldPosition);

            if (Physics.Raycast(HookShotGun.position, TargetWorldPosition, out hit, 200.0f, layerMask))
            {
                newPosition = MainCamera.WorldToScreenPoint(hit.point);
            }

            AimReticleReal.position = newPosition;
        }
        else
        {
        }
    } 

    public Vector3 GetHookTargetPosition()
    {
        // Layer 8 is defined as "Hookshotable" so we want layerMask to represent only that layer
        return MainCamera.transform.forward;
        int layerMask = 1 << 8;
        RaycastHit hit;
        Ray VirtualRay = MainCamera.ScreenPointToRay(AimReticleReal.position);
        if (Physics.Raycast(VirtualRay, out hit, 200.0f, layerMask))
        {
            Vector3 TargetWorldPosition = hit.point - HookShotGun.position;
            return TargetWorldPosition;
        }
        return PlayerToTarget;
    }
}

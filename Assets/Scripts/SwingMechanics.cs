using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingMechanics : MonoBehaviour
{
    [SerializeField] private Transform HookAnchor, PlayerAnchor, GrappleGun;

    private Rigidbody PlayerRb, HookedRb;

    private LineRenderer TetherRenderer = null;

    public bool tethered = false;

    [SerializeField] private float playerDist = 0.0f;
    Vector3 acceleration;
    Vector3 lastVelocity;

    private float initEnergy = 0.0f;

    [SerializeField] private float minTensionLength;
    Vector3 PlayerToHook;
    private float stringConst = 0.1f;


    void Awake()
    {
        TetherRenderer = GetComponent<LineRenderer>();
        TetherRenderer.positionCount = 2;

        PlayerRb = PlayerAnchor.GetComponentInParent<Rigidbody>();
        lastVelocity = PlayerRb.velocity;

        HookedRb = HookAnchor.GetComponentInParent<Rigidbody>();
        minTensionLength = 4.0f;
    }

    void FixedUpdate()
    {
        PlayerToHook = HookAnchor.position - PlayerAnchor.position;

        TetherRenderer.positionCount = 2;

        TetherRenderer.SetPosition(0, HookAnchor.position);
        TetherRenderer.SetPosition(1, GrappleGun.position);

        if (!tethered)
        {
            minTensionLength = 0;
            return;
        }

        acceleration = (PlayerRb.velocity - lastVelocity) / Time.fixedDeltaTime;


        playerDist = PlayerToHook.magnitude;

        Vector3 CentForce = PlayerToHook.normalized * PlayerRb.velocity.sqrMagnitude / playerDist;

        float cosVal = (HookAnchor.position.y - PlayerAnchor.position.y) / playerDist;

        Vector3 TensionForce = cosVal * Physics.gravity.magnitude * PlayerToHook.normalized;

        if (playerDist < minTensionLength) TensionForce = new Vector3();
        else if (playerDist >= minTensionLength && playerDist <= minTensionLength + 0.5f)
        {
            int constMul = -1;
            Vector3 ProjectedV = Vector3.Project(acceleration, PlayerToHook.normalized);
            if (Vector3.Angle(PlayerToHook, ProjectedV) > 90)
            {
                PlayerRb.AddForceAtPosition(ProjectedV * constMul,
                    GrappleGun.position, ForceMode.Acceleration);
                HookedRb.AddForce(ProjectedV * constMul * -1);
            }
        }
        else
        {
            float distFromTensionL = playerDist - minTensionLength;
            if (distFromTensionL > 2) distFromTensionL = 2.0f;
            PlayerRb.AddForceAtPosition(PlayerToHook.normalized * stringConst * distFromTensionL, 
                GrappleGun.position, ForceMode.Acceleration);
            HookedRb.AddForce(PlayerToHook.normalized * -1 * stringConst * distFromTensionL);
        } 

        PlayerRb.AddForceAtPosition(TensionForce + CentForce,
            GrappleGun.position, ForceMode.Acceleration);
        HookedRb.AddForce((TensionForce + CentForce) * -1);
    }

    public void UpdateTensionDistance()
    {
        Vector3 PlayerToHook = HookAnchor.position - PlayerAnchor.position;
        minTensionLength = PlayerToHook.magnitude;
    }

    public void ReelTether(int reelSpeed)
    {
        if (minTensionLength <= .5f) minTensionLength = .5f;
        minTensionLength -= reelSpeed * Time.fixedDeltaTime;
        PlayerRb.AddForceAtPosition(PlayerToHook.normalized * reelSpeed, 
            GrappleGun.position, ForceMode.Acceleration);
        if (playerDist < minTensionLength) minTensionLength = playerDist;
    }

    public void SetTethered()
    {
        tethered = true;
        UpdateTensionDistance();
    }

    public void DetachTether()
    {
        tethered = false;
    }
}

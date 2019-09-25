using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SwingMechanics HookShot;
    [SerializeField] private Transform RespawnAnchor;

    private Rigidbody PlayerRb = null;
    private Camera MainCamera = null;
    [SerializeField] private bool isGrounded = false;

    [SerializeField] private float ForceX, ForceY, ForceZ, jumpForce;
    [SerializeField] private float maxForceX, maxForceY, maxForceZ;

    [SerializeField] private Transform GroundNormal;

    public HookObject Hook;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerRb = GetComponent<Rigidbody>();
        MainCamera = Camera.main;
        GroundNormal = GameObject.FindGameObjectWithTag("Initial Ground").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            HookShot.UpdateTensionDistance();

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = RespawnAnchor.position;
            PlayerRb.velocity = new Vector3(0, -2, 0);
            transform.eulerAngles = new Vector3();
        }

        if (Input.GetKeyDown(KeyCode.Q))
            Hook.LaunchHook(transform.up);
        else if(!Input.GetKey(KeyCode.Q))
            Hook.DetachHook();
            
        if (Input.GetKey(KeyCode.A))
            ForceX -= Time.deltaTime * 10;
        if (Input.GetKey(KeyCode.D))
            ForceX += Time.deltaTime * 10;
        if(!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            ForceX = ForceX / 2;


        if (Input.GetKey(KeyCode.S))
            ForceZ -= Time.deltaTime * 10;
        if (Input.GetKey(KeyCode.W))
            ForceZ += Time.deltaTime * 10;
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            ForceZ = ForceZ / 2;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            ForceY += jumpForce;
        else ForceY = 0;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift) && HookShot.tethered)
            HookShot.ReelTether(2);

        if (isGrounded)
        {
            Vector3 VectorOnPlane;
            if (ForceX != 0.0f)
            {
                VectorOnPlane = Vector3.ProjectOnPlane(MainCamera.transform.right,
                            GroundNormal.up).normalized;
                if (ForceX > maxForceX)
                    ForceX = maxForceX;
                
                Vector3 XAxis = VectorOnPlane * ForceX;
                PlayerRb.AddForce(XAxis, ForceMode.Acceleration);
            }
            if (ForceY != 0.0f)
            {
                if (ForceY > maxForceY)
                    ForceY = maxForceY;

                    Vector3 YAxis = GroundNormal.up * ForceY;
                    PlayerRb.AddForce(YAxis, ForceMode.Acceleration);
            }
            if (ForceZ != 0.0f)
            {
                VectorOnPlane = Vector3.ProjectOnPlane(MainCamera.transform.forward,
                                            GroundNormal.up).normalized;
                if (ForceZ > maxForceZ)
                    ForceZ = maxForceZ;

                Vector3 ZAxis = VectorOnPlane * ForceZ;
                PlayerRb.AddForce(ZAxis, ForceMode.Acceleration);
            }
        }
    }

    private bool FindNewFloor()
    {

        return false;
    }

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
    }

    public void SetGroundNormal(Transform tr)
    {
        if(GroundNormal != tr)
            GroundNormal = tr;
    }

    public void ClearGround()
    {
        GroundNormal.GetComponent<SurfaceObject>().isCurrentlyFloor = false;
    }
}

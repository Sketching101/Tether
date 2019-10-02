using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGrounded : MonoBehaviour
{
    private PlayerController PlayerObj;

    int layerMask = 0;

    [SerializeField] int GroundedCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = PlayerController.player; //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        layerMask = 8;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == layerMask)
        {
            GroundedCount++;
            PlayerObj.SetGrounded(true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == layerMask)
        {
            GroundedCount--;
            if (GroundedCount == 0)
                PlayerObj.SetGrounded(false);
            else if (GroundedCount < 0)
                Debug.Log("Grounded count is less than 0! Should not happen");
        }
    }
}

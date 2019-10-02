using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceObject : MonoBehaviour
{
    public bool isCurrentlyFloor = false;
    private PlayerController Player = null;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(isCurrentlyFloor)
        {
    //        Physics.gravity = transform.up * -1 * 9.81f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player.ClearGround();
            Player.SetGroundNormal(transform);
            isCurrentlyFloor = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isCurrentlyFloor = false;
        }
    }
}

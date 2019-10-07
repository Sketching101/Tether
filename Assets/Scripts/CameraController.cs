using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController Player;
    [SerializeField] private Transform mainCamera, cameraAnchor;

    Vector3 PlayerLastPos;
    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main.transform;
        PlayerLastPos = Player.transform.position;
        cameraAnchor.position = mainCamera.position;
    }

    // Update is called once per frame
    void Update()
    {
        cameraAnchor.RotateAround(Player.transform.position, Player.transform.up, Input.GetAxis("Mouse X"));
        cameraAnchor.RotateAround(Player.transform.position, Player.transform.right, -1 * Input.GetAxis("Mouse Y"));

        cameraAnchor.position += Player.transform.position - PlayerLastPos;

        mainCamera.position = cameraAnchor.position;

        mainCamera.LookAt(Player.transform);

        PlayerLastPos = Player.transform.position;
    }
}


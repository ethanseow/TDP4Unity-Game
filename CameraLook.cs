
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Camera cam;

    [Range(0, 0.5f)] public float dampSpeed;

    private const float Z_POS = -10;

    private Vector3 pos;
    private Vector3 dir; //direction from camera to player

    void Start()
    {
        cam = GetComponent<Camera>();
    }
    void FixedUpdate()
    {
        pos = new Vector3(player.transform.position.x, player.transform.position.y, Z_POS);
        dir = (pos - transform.position).normalized;

        transform.position = Vector3.SmoothDamp(transform.position, pos, ref dir, dampSpeed);
    }
}
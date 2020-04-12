using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Range(0, 10)] public float dampSpeed;
    //[Range(0, 10)] public float moveSpeed;

    private Vector2 dir;

    void Update()
    {
        dir = (transform.position - player.transform.position).normalized;
        transform.position = Vector2.SmoothDamp(transform.position, player.transform.position, ref dir, dampSpeed);
    }
}

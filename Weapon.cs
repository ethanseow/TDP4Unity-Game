using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [Range(0, 10f)] [SerializeField] float speed = 0;
    [Range(0, 10f)] [SerializeField] float range = 0;
    [Range(0, 10f)] [SerializeField] float fireRate = 0;

    private Vector2 startingPos;
    void Start()
    {
        startingPos = new Vector2(transform.position.x, transform.position.y);
    }
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        if (!inRange(startingPos))
        {
            Destroy();
        }
    }
    private bool inRange(Vector2 startingPos)
    {
        return Vector2.Distance(startingPos, new Vector2(transform.position.x, transform.position.y)) < range;
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
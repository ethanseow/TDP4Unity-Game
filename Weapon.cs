using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [Range(0, 100f)] [SerializeField] private float speed;
    [Range(0, 100f)] [SerializeField] private float range;
    [Range(0, 10f)] [SerializeField] private float fireRate; //fires a shot every x seconds


    private Vector2 startingPos; //playerPos in Vector2
    void Start()
    {
        startingPos = new Vector2(transform.position.x, transform.position.y);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        if (!inRange(startingPos))
        {
            destroy();
        }
    }

    private bool inRange(Vector2 startingPos)
    {
        return Vector2.Distance(startingPos, new Vector2(transform.position.x, transform.position.y)) < range;
    }

    private void destroy()
    {
        Destroy(gameObject);
    }

    public float getFireRate()
    {
        return fireRate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        destroy();
    }
}
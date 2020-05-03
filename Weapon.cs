using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponStats stats;
    private Vector2 startingPos; //playerPos in Vector2

    private SpriteRenderer projectileRend;
    void Start()
    {
        startingPos = new Vector2(transform.position.x, transform.position.y);
        projectileRend = GetComponent<SpriteRenderer>();
        projectileRend.sprite = stats.projectileSprite;
    }

    void Update()
    {
        transform.Translate(Vector2.up * stats.speed * Time.deltaTime);
        if (!inRange()) {
            destroy();
        }
    }
    
    private bool inRange()
    {
        return Vector2.Distance(startingPos, new Vector2(transform.position.x, transform.position.y)) < stats.range;
    }

    private void destroy()
    {
        Destroy(gameObject);
    }

    public float getFireRate()
    {
        return stats.fireRate;
    }

    public float getRecoil()
    {
        return stats.maxRecoil;
    }
    
    public Sprite getGunSprite()
    {
        return stats.weaponSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        destroy();
    }
}

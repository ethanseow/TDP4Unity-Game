using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControl : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject crosshair;

    [SerializeField] private GameObject gun; //Child of player object that holds the gun
    private SpriteRenderer gunRend;

    private Weapon weaponStats; //Weapon code

    private bool canShoot;
    
    private Vector2 cursorPos;
    private Vector2 playerPos; //transform.position in Vector2
    private Vector2 diff; //difference between cursor and player

    private float recoilOffset;
    private float shootAngle; //angle mouse is facing
    [Range(-180f, 180f)] public float offset; //angle offset of projectile

    void Start()
    {
        canShoot = true;
        weaponStats = weapon.GetComponent<Weapon>();

        Cursor.visible = false;
        crosshair = Instantiate(crosshair);
        gunRend = gun.GetComponent<SpriteRenderer>();
        gunRend.sprite = weaponStats.getGunSprite();
    }

    void Update()
    {

        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerPos = new Vector2(transform.position.x, transform.position.y);
        diff = cursorPos - playerPos;
        shootAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        setRecoilOffset();

        crosshair.transform.position = cursorPos;

        if (Mathf.Abs(shootAngle) >= 90)
        {
            gun.transform.rotation = Quaternion.Euler(180, 0, -shootAngle);
        }
        else
        {
            gun.transform.rotation = Quaternion.Euler(0, 0, shootAngle);
        }

        //gun.transform.position = playerPos + diff.normalized;
    }

    private void setRecoilOffset()
    {
        float maxRecoil = weaponStats.getRecoil();

        recoilOffset = maxRecoil / (1 + Mathf.Pow(maxRecoil - 1, -0.2f * distanceFromMouse() + 2));
    }

    private float recoilAngle()
    {
        return Random.Range(-recoilOffset, recoilOffset);
    }

    private float distanceFromMouse()
    {
        return Vector2.Distance(transform.position, cursorPos);
    }

    public void Fire()
    {
        canShoot = false;
        StartCoroutine(shotTimer());
        GameObject projectile = Instantiate(weapon, playerPos + diff.normalized, Quaternion.Euler(0f, 0f, getShootAngle() + recoilAngle()));
    }
 

    public bool canFire()
    {
        return canShoot;
    }
    public float getShootAngle()
    {
        return shootAngle + offset;
    }

    IEnumerator shotTimer()
    {
        yield return new WaitForSeconds(weaponStats.getFireRate());
        canShoot = true;
    }
}

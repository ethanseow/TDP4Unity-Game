using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControl : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    private Weapon weaponStats;

    private bool canShoot;
    private bool timing;

    void Start()
    {
        canShoot = true;
        weaponStats = weapon.GetComponent<Weapon>();

    }

    public void Fire(Vector2 pos, float angle)
    {
        canShoot = false;
        StartCoroutine(shotTimer());
        GameObject projectile = Instantiate(weapon, pos, Quaternion.Euler(0f, 0f, angle));
    }

    public bool canFire()
    {
        return canShoot;
    }

    IEnumerator shotTimer()
    {
        yield return new WaitForSeconds(weaponStats.getFireRate());
        canShoot = true;
    }
}
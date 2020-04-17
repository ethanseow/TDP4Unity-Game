using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControl : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    private Weapon weaponStats;

    private bool canShoot;
    
    private Vector2 cursorPos;

    private float recoilOffset;

    void Start()
    {
        canShoot = true;
        weaponStats = weapon.GetComponent<Weapon>();
    }

    void Update()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        setRecoilOffset();
        Debug.Log(distanceFromMouse());
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


    public void Fire(Vector2 pos, float angle)
    {
        canShoot = false;
        StartCoroutine(shotTimer());
        GameObject projectile = Instantiate(weapon, pos, Quaternion.Euler(0f, 0f, angle + recoilAngle()));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControl : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    private Weapon weaponStats;

    private int shotCount;
    private float timer = 0; //if timer exceeds 1 second then reset shotCount

    void Start()
    {
        shotCount = 0;
        weaponStats = weapon.GetComponent<Weapon>();
        StartCoroutine(shotTimer());
    }

    public void Fire(Vector2 pos, float angle)
    {
        shotCount++;
        GameObject projectile = Instantiate(weapon, pos, Quaternion.Euler(0f, 0f, angle));
    }

    public int getShotCount()
    {
        return shotCount;
    }
    public int getRate()
    {
        return weaponStats.getFireRate();
    }

    IEnumerator shotTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            shotCount = 0;
        }
    }
}

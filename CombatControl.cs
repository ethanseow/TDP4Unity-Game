using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControl : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    private Weapon weaponStats;

    void Start()
    {
        weaponStats = GetComponent<Weapon>();
    }

    public void Fire(Vector2 pos, float angle)
    {
        GameObject projectile = Instantiate(weapon, pos, Quaternion.Euler(0f, 0f, angle));
    }
}

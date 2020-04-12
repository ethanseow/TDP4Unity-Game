using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControl : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    void Start()
    {

    }

    public void Fire(Vector2 dir, float angle)
    {
        GameObject projectile = Instantiate(weapon, dir, Quaternion.Euler(0f, 0f, angle));
    }
}
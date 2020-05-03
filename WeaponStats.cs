using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/New_Weapon")]
public class WeaponStats : ScriptableObject
{
    public Sprite weaponSprite;
    public Sprite projectileSprite;

    [Range(0, 100f)] public float speed;
    [Range(0, 100f)] public float range;
    [Range(0, 10f)] public float fireRate; //fires a shot every x seconds
    [Range(1, 30f)] public float maxRecoil; //projectile travels in random angle between 0 and x 
    public float numBullets; //number of bullets fired in one click

}

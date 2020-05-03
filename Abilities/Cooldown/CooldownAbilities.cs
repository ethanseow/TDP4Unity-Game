using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CooldownAbilities : MonoBehaviour
{
    public GameObject player;
    public string Name;

    public abstract void Start_Ability(GameObject player);
}

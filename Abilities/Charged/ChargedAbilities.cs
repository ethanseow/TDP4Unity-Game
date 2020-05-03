using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChargedAbilities : MonoBehaviour
{
    public GameObject player;
    public string Name;

    public abstract void Start_Ability(Vector3 pos);
}

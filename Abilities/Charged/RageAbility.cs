using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageAbility : ChargedAbilities
{
    /*Rage - Charge
     * Creates resource bar that capping at 300
     * Resource increases as player deals damage to other players
     * Damage from this ability does not increase bar
     * Each use costs 100 resource
     * 
     * Exponentially decreases as time goes on an damage has not been dealt
     */

    private const float maxRage = 300f;
    private float rage;

    private float timeBeforeAttack;

    private PlayerMovement pMove;


    void Start()
    {
        Name = "Rage";
    }
    void Update()
    {
        pMove = player.GetComponent<PlayerMovement>();
        rage = Mathf.Min(rage, maxRage);
    }

    public override void Start_Ability(Vector3 pos)
    {
        Debug.Log("Rage");
    }
}

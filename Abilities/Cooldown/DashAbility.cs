using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : CooldownAbilities
{
    private Vector2 direction;
    private Rigidbody2D rb;

    [SerializeField] float dashSpeed;
    void Start()
    {

    }

    public override void Start_Ability(GameObject player)
    {
        rb = player.GetComponent<Rigidbody2D>();
        Debug.Log("Dash");
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        rb.velocity = dashSpeed * new Vector2(direction.x, direction.y).normalized;
    }
}
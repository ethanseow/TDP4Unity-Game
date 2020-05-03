﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthClaymoreAbility : ChargedAbilities
{
    private RaycastHit hit;
    private BoxCollider2D playerCollider;
    private SpriteRenderer sprite;
    private bool loadingTrap = false;
    private bool startedTimer = false;
    private bool touchedSomething = false;
    private Rigidbody2D rb;
    private float count = 1;
    private float throwSpeed = 0;
    private float throwHeight = 0;
    private float maxHeight = int.MinValue;
    private float initMouseHeight;

    [SerializeField] float amt = 0;
    [SerializeField] float timespeed = 1;
    [SerializeField] float loadingTime = 0;
    [SerializeField] float percentSlow = 0;
    [SerializeField] LayerMask groundLayer = 0;
    [SerializeField] LayerMask playerLayer = 0;


    private void Start()
    {


        Name = "Stealth Claymore"; //Sets Ability name 


        throwSpeed = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        throwHeight = (Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y) * 2;
        if (throwHeight < 0)
        {
            count = 0;
            throwHeight *= -1;
        }
        else
        {
            initMouseHeight = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            var currAmt = (initMouseHeight - transform.position.y) * 0.1447273f;
            transform.position = new Vector2(transform.position.x, transform.position.y + currAmt);
        }
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(throwSpeed, count * throwHeight);
        sprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
    }
    private void FixedUpdate()
    {
        if (!touchedSomething)
        {
            maxHeight = Mathf.Max(maxHeight, transform.position.y);
            count -= Time.fixedDeltaTime * timespeed;
            rb.velocity = new Vector2(throwSpeed * timespeed, count * timespeed * throwHeight);
            return;
        }
        if (loadingTrap)
        {
            return;
        }
        else if (!startedTimer)
        {
            StartTimer(loadingTime);
            startedTimer = true;
            rb.velocity = Vector2.zero;
            return;
        }
        RaycastHit2D boxCast = Physics2D.BoxCast(transform.position, new Vector2(1, 0.01f), 0, Vector2.up, 0.2f, playerLayer);
        if (boxCast.collider != null)
        {
            var currSpeed = boxCast.collider.GetComponent<PlayerMovement>().GetMovementSpeed();
            Debug.Log(currSpeed);
            boxCast.collider.GetComponent<PlayerMovement>().SetMovementSpeed(currSpeed * 0.01f * percentSlow);
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        touchedSomething = true;
    }
    private void StartTimer(float time)
    {
        playerCollider = GetComponent<BoxCollider2D>();
        IEnumerator coroutine = PauseTime(time);
        StartCoroutine(coroutine);
        sprite.color = Color.red;

    }
    private IEnumerator PauseTime(float time)
    {
        loadingTrap = true;
        yield return new WaitForSecondsRealtime(time);
        loadingTrap = false;
    }

    public override void Start_Ability(Vector3 pos)
    {
        Debug.Log("Bomb");
        GameObject basicAbilityGameObject = Instantiate(this.gameObject, pos, new Quaternion(0, 0, 0, 0));
    }
}
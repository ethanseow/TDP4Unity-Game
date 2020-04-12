using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 horizontalMovement = Vector2.zero;
    private Vector2 playerMovement = Vector2.zero;
    private Vector2 verticalMovement = Vector2.zero;
    private Transform playerTransform;

    private CombatControl combatController;

    public float jumpHeight;
    public float movementSpeed;
    public float gravity;
    private float count;
    private Vector2 cursorPos;
    private Vector2 playerPos; //player position in Vector2
    [Range(-180f, 180f)] public float offset; //angle offset of projectile
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        combatController = GetComponent<CombatControl>();
        playerTransform = GetComponent<Transform>();
    }

    void Update()
    {
        Aim();
    }
    void FixedUpdate()
    {
        MovementInput();
        SetMovement();
    }

    void MovementInput()
    {
        horizontalMovement = Vector2.right * Input.GetAxisRaw("Horizontal") * movementSpeed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            verticalMovement = Vector2.up * jumpHeight;
            count = 0;
        }
        else
        {
            verticalMovement -= Vector2.up * gravity * count;
        }
        playerMovement = horizontalMovement + verticalMovement;
        playerMovement = playerMovement * Time.deltaTime;
        count += Time.fixedDeltaTime;
    }
    void SetMovement()
    {
        rb.MovePosition(new Vector2(rb.position.x, transform.position.y) + playerMovement);
    }
    void Aim()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 diff = cursorPos - playerPos;
        float shootAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            combatController.Fire(playerTransform.position, shootAngle + offset);
        }
    }
}
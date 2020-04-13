using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 horizontalMovement = Vector2.zero;
    private Vector2 playerMovement = Vector2.zero;
    private Vector2 verticalMovement = Vector2.zero;

    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    private Transform playerTransform;

    private CombatControl combatController;

    private bool isJumping;

    private Vector2 cursorPos;
    private Vector2 playerPos;
    [Range(-180f, 180f)] public float offset;

    [SerializeField] LayerMask groundLayer = 0;
    [SerializeField] float jumpHeight = 0;
    [SerializeField] float movementSpeed = 0;
    [SerializeField] float gravity = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        combatController = GetComponent<CombatControl>();
        playerTransform = GetComponent<Transform>();
    }
    private void Update()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 diff = cursorPos - playerPos;
        float shootAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (Input.GetKey(KeyCode.Mouse0) && combatController.canFire())
        {
            combatController.Fire(playerTransform.position, shootAngle + offset);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isJumping = true;
        }
    }
    private void FixedUpdate()
    {
        MovementInput();
        SetMovement();
    }
    private void MovementInput()
    {
        if (isGrounded())
        {
            if (isJumping)
            {
                verticalMovement = Vector2.up * jumpHeight;
            }
            else
            {
                verticalMovement = Vector2.down;
            }
        }
        else
        {
            verticalMovement -= gravity * Vector2.up;
            isJumping = false;
        }
        horizontalMovement = Vector2.right * Input.GetAxisRaw("Horizontal") * movementSpeed;
        playerMovement = horizontalMovement + verticalMovement;
        playerMovement = playerMovement * Time.deltaTime;
    }
    private void SetMovement()
    {
        rb.MovePosition(new Vector2(rb.position.x, transform.position.y) + playerMovement);
    }
    private bool isGrounded()
    {
        RaycastHit2D groundCheck = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down, playerCollider.bounds.size.y * 0.05f, groundLayer);
        Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.size.y));
        return groundCheck.collider != null;
    }
    public void SetMovementSpeed(float setMovespeed)
    {
        movementSpeed = setMovespeed;
    }
    public float GetMovementSpeed()
    {
        return movementSpeed;
    }
    
}
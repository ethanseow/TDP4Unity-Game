
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    private Vector2 horizontalMovement = Vector2.zero;
    private Vector2 playerMovement = Vector2.zero;
    private Vector2 verticalMovement = Vector2.zero;
    private CombatControl combatController;

    //Physics
    public float jumpHeight;
    public float movementSpeed;
    public float gravity;

    //If space if pressed
    private bool jump;

    private Vector2 cursorPos;
    private Vector2 playerPos; //player position in Vector2
    [Range(-180f, 180f)] public float offset; //angle offset of projectile

    //Groundcheck
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundChecker;
    [Range(0, 1)][SerializeField] private float rayLength;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        combatController = GetComponent<CombatControl>();
    }

    void Update()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 diff = cursorPos - playerPos;
        float shootAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (Input.GetKeyDown(KeyCode.Mouse0) && combatController.canFire())
        {
            combatController.Fire(playerPos + diff.normalized, shootAngle + offset);
        }

        Debug.Log(combatController.canFire());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }
    void FixedUpdate()
    {
        
        Jump();
        MovementInput();
        SetMovement();
    }

    void MovementInput()
    {
        horizontalMovement = Vector2.right * Input.GetAxisRaw("Horizontal") * movementSpeed;

        playerMovement = horizontalMovement + verticalMovement;
        playerMovement = playerMovement * Time.deltaTime;
    }

    void SetMovement()
    {
        rb.MovePosition(new Vector2(rb.position.x, transform.position.y) + playerMovement);
    }
    void Jump()
    {
        if (isGrounded())
        {
            if (jump)
            {
                verticalMovement = Vector2.up * jumpHeight;
            }
            else
            {
                verticalMovement = Vector2.zero;
            }
        }
        else
        {
            verticalMovement -= Vector2.up * gravity;
            jump = false;
        }

        if (topCollision())
        {
            verticalMovement = Vector2.zero;
            verticalMovement -= Vector2.up * gravity;
        }

    }
    private bool isGrounded()
    {
        RaycastHit2D groundCheck = Physics2D.BoxCast(groundChecker.position, playerCollider.bounds.size, 0, Vector2.down, playerCollider.bounds.size.y * rayLength, groundLayer);
        Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.size.y * rayLength));

        return groundCheck.collider != null;
    }
    private bool topCollision()
    {
        RaycastHit2D topCheck = Physics2D.BoxCast(groundChecker.position, playerCollider.bounds.size, 0, Vector2.up, playerCollider.bounds.size.y * rayLength, groundLayer);

        return topCheck.collider != null;
    }
}
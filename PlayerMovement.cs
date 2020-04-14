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
    private bool canMove = true;
    private bool isMoving = false;

    private Vector2 cursorPos;
    private Vector2 playerPos; //player position in Vector2
    [Range(-180f, 180f)] public float offset; //angle offset of projectile

    //Groundcheck
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask slopeLayer;
    [SerializeField] private LayerMask slopeLayer2;
    [SerializeField] private Transform groundChecker;
    [Range(0, 1)] [SerializeField] private float rayLength;


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
        if (Input.GetKey(KeyCode.Mouse0) && combatController.canFire())
        {
            combatController.Fire(playerPos + diff.normalized, shootAngle + offset);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }
    void FixedUpdate()
    {
        if (!canMove) { return; }
        Jump();
        MovementInput();
        SetMovement();
    }

    void MovementInput()
    {
        horizontalMovement = Vector2.right * Input.GetAxisRaw("Horizontal") * movementSpeed;
        if(horizontalMovement != Vector2.zero) {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        playerMovement = horizontalMovement + verticalMovement;
        playerMovement = playerMovement * Time.deltaTime;
    }

    void SetMovement()
    {
        rb.MovePosition(new Vector2(rb.position.x, rb.position.y) + playerMovement);
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

        if (collidingWithTop())
        {
            verticalMovement = Vector2.zero;
            verticalMovement -= Vector2.up * gravity;
        }
        if (!jump)
        {
            isGrounded();
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D groundCheck = Physics2D.BoxCast(groundChecker.position, playerCollider.bounds.size, 0, Vector2.down, playerCollider.bounds.size.y * rayLength, groundLayer);
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.5f, -0.5f, 0), Vector2.down, 1 + rayLength, slopeLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(-0.5f, -0.5f, 0), Vector2.down, 1 + rayLength, slopeLayer2);
        Debug.DrawLine(transform.position + new Vector3(0.5f,-0.5f,0), transform.position + new Vector3(0.5f, -0.5f, 0) + (playerCollider.bounds.size.y + rayLength) * Vector3.down, Color.red);
        
        if (hit.collider != null)
        {
            float distance = (transform.position + new Vector3(0.5f, -0.5f, 0)).y - hit.point.y;
            Debug.Log(distance);
            verticalMovement = Vector2.up * -70 * distance;
            return true;
        }else if (hit2.collider != null)
        {
            float distance = (transform.position + new Vector3(-0.5f, -0.5f, 0)).y - hit2.point.y;
            Debug.Log(distance);
            verticalMovement = Vector2.up * -70 * distance;
            return true;
        }
        return groundCheck.collider != null;
    }
    private bool collidingWithTop()
    {
        RaycastHit2D topCheck = Physics2D.BoxCast(groundChecker.position, playerCollider.bounds.size, 0, Vector2.up, playerCollider.bounds.size.y * rayLength, groundLayer);

        return topCheck.collider != null;
    }

    public void SetMovementSpeed(float setMovespeed)
    {
        movementSpeed = setMovespeed;
    }
    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public void SetAbleToMove(bool ableToMove)
    {
        canMove = ableToMove;
    }
}
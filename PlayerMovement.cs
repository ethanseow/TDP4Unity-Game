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
    private float prevRayLength;

    //Physics
    public float jumpHeight;
    public float movementSpeed;
    public float gravity;


    //If space if pressed
    private bool jump;
    private bool canMove = true;
    private bool isMoving = false;
    private bool wasOnSlope = false;


    private Vector2 cursorPos;
    private Vector2 playerPos; //player position in Vector2
    [Range(-180f, 180f)] public float offset; //angle offset of projectile

    //Groundcheck
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask slopeLayer;
    [SerializeField] private LayerMask slopeLayer2;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float slideSpeedMultiplier;
    [Range(0, 1)] [SerializeField] private float rayLength;
    [Range(0, 20)] [SerializeField] private float slopeRayLength;


    void Start()
    {
        prevRayLength = slopeRayLength;
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
        Vector2 movementVector = Vector3.right;
        float leftRightMovement = Input.GetAxisRaw("Horizontal");
        RaycastHit2D hitInfoRight = Physics2D.Raycast(transform.position, Vector2.down, slopeRayLength, slopeLayer | slopeLayer2);
        Debug.DrawRay(transform.position, Vector2.down * slopeRayLength);

        if (hitInfoRight.collider != null)
        {
            float angle = hitInfoRight.collider.transform.eulerAngles.z;
            if (360 - angle < 180)
            {
                angle = -(360 - angle);
            }
            float slideSpeed = (-angle / 90) * slideSpeedMultiplier;
            movementVector = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
            if (angle < 0 && leftRightMovement < 0 || angle > 0 && leftRightMovement > 0)
            {
                // going up ramp, slower
                leftRightMovement += slideSpeed;
            }
            else if (angle > 0 && leftRightMovement < 0 || angle < 0 && leftRightMovement > 0)
            {
                // going down ramp, faster
                leftRightMovement -= slideSpeed;
            }
            else
            {
                // slide down
                leftRightMovement = slideSpeed;
            }
        }
        horizontalMovement = movementVector * leftRightMovement * movementSpeed;
        if (horizontalMovement != Vector2.zero)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        playerMovement = horizontalMovement + verticalMovement;
        playerMovement = playerMovement * Time.fixedDeltaTime;
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
                slopeRayLength = 0;
            }
            else
            {
                verticalMovement = Vector2.zero;
                slopeRayLength = prevRayLength;
            }
        }
        else
        {
            verticalMovement -= Vector2.up * gravity;
            jump = false;
        }
        if (collidingWithTop())
        {
            if (verticalMovement.y > 0)
            {
                verticalMovement = Vector2.zero;
            }
            verticalMovement -= Vector2.up * gravity;
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D groundCheck = Physics2D.BoxCast(groundChecker.position, playerCollider.bounds.size, 0, Vector2.down, playerCollider.bounds.size.y * rayLength * 2, groundLayer | slopeLayer | slopeLayer2);
        return groundCheck.collider != null;
    }
    private bool collidingWithTop()
    {
        RaycastHit2D topCheckSlopeAndGround = Physics2D.BoxCast(groundChecker.position, playerCollider.bounds.size, 0, Vector2.up, playerCollider.bounds.size.y * rayLength, slopeLayer | groundLayer | slopeLayer2);
        return topCheckSlopeAndGround.collider != null;
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
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
        if (isGrounded() || isSlopeGrounded())
        {
            if (jump)
            {
                verticalMovement = Vector2.up * jumpHeight;
                slopeRayLength = 0;
            }
            else
            {
                verticalMovement = Vector2.zero;
                isSlopeGrounded();
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
            if (verticalMovement.y > 0){
                verticalMovement = Vector2.zero;
            }
            verticalMovement -= Vector2.up * gravity;
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D groundCheck = Physics2D.BoxCast(groundChecker.position, playerCollider.bounds.size, 0, Vector2.down, playerCollider.bounds.size.y * rayLength * 2, groundLayer);
        return groundCheck.collider != null;
    }

    private bool isSlopeGrounded()
    {
        Vector3 bottomRightPointCollider = new Vector3(transform.localScale.x / 2, -transform.localScale.y / 2, 0);
        Vector3 bottomLeftPointCollider = new Vector3(-transform.localScale.x / 2, -transform.localScale.y / 2, 0);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + bottomRightPointCollider, Vector2.down, slopeRayLength, slopeLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + bottomLeftPointCollider, Vector2.down, slopeRayLength, slopeLayer2);
        RaycastHit2D slopeCheck = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, Vector2.down, playerCollider.bounds.size.y * rayLength / 2, slopeLayer | slopeLayer2);
        // Debug.DrawLine(transform.position + bottomRightPointCollider, transform.position + bottomRightPointCollider + (slopeRayLength) * Vector3.down, Color.red);
        // Debug.DrawLine(transform.position + bottomLeftPointCollider, transform.position + bottomLeftPointCollider + (slopeRayLength) * Vector3.down, Color.red);
        if (slopeCheck.collider != null)
        {
            var tempWasOnSlope = wasOnSlope;
            wasOnSlope = true;
            if (!tempWasOnSlope)
            {
                return true;
            }
        }
        if (rightHit.collider != null && wasOnSlope)
        {
            float distance = (transform.position + bottomRightPointCollider).y - rightHit.point.y;
            verticalMovement = Vector2.up * -70 * distance;
            return true;
        }
        else if (leftHit.collider != null && wasOnSlope)
        {
            float distance = (transform.position + bottomLeftPointCollider).y - leftHit.point.y;
            verticalMovement = Vector2.up * -70 * distance;
            return true;
        }
        wasOnSlope = false;
        return false;
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
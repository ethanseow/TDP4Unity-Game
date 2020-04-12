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
    private Transform playerTransform;

    private CombatControl combatController;

    public float jumpHeight;
    public float movementSpeed;
    public float gravity;

    private Vector2 cursorPos;
    private Vector2 playerPos; //player position in Vector2
    [Range(-180f, 180f)] public float offset; //angle offset of projectile

    //Groundcheck
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        combatController = GetComponent<CombatControl>();
        playerTransform = GetComponent<Transform>();
    }

    void Update()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 diff = cursorPos - playerPos;
        float shootAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            combatController.Fire(playerTransform.position, shootAngle + offset);
        }
        Debug.Log("Grounded: " + isGrounded());

        
    }
    void FixedUpdate()
    {
        MovementInput();
        SetMovement();
        Jump();
    }

    void MovementInput()
    {
        horizontalMovement = Vector2.right * Input.GetAxisRaw("Horizontal") * movementSpeed;
        
        playerMovement = horizontalMovement + verticalMovement;
        playerMovement = playerMovement * Time.deltaTime;
        //counter += Time.fixedDeltaTime;
    }

    void SetMovement()
    {
        rb.MovePosition(new Vector2(rb.position.x, transform.position.y) + playerMovement);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            verticalMovement = Vector2.up * jumpHeight;
        }
        else
        {
            verticalMovement -= Vector2.up * gravity;
            
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D groundCheck = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down, playerCollider.bounds.size.y * 0.2f, groundLayer);
        Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.size.y));

        return groundCheck.collider != null;
    }
}

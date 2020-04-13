using System.Collections;
using UnityEngine;

public class StealthClaymoreAbility : MonoBehaviour
{
    private RaycastHit hit;
    private BoxCollider2D playerCollider;
    private SpriteRenderer sprite;
    private bool loadingTrap = false;
    private bool startedTimer = false;
    private Rigidbody2D rb;


    [SerializeField] float loadingTime = 0;
    [SerializeField] float percentSlow = 0;
    [SerializeField] LayerMask groundLayer = 0;
    [SerializeField] LayerMask playerLayer = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0.5f, 0.5f));
        sprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();

    }
    private void FixedUpdate()
    {
        if (!isGrounded()) { return; }
        if (loadingTrap) {
            return;
        } 
        else if(!startedTimer) {
            StartTimer(loadingTime);
            startedTimer = true;
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
    private bool isGrounded()
    {
        RaycastHit2D groundCheck = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down, playerCollider.bounds.size.y * 0.05f, groundLayer);
        Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.size.y));
        return groundCheck.collider != null;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Vector2 direction;
    private Rigidbody2D rb;

    [SerializeField] GameObject player;
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody2D>();
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        rb.velocity = dashSpeed * new Vector2(direction.x, direction.y).normalized;
        StartTimer(dashTime);
    }

    private void StartTimer(float time)
    {
        IEnumerator coroutine = PauseTime(time);
        StartCoroutine(coroutine);
    }
    private IEnumerator PauseTime(float time)
    {
        player.GetComponent<PlayerMovement>().SetAbleToMove(false);
        yield return new WaitForSecondsRealtime(time);
        player.GetComponent<PlayerMovement>().SetAbleToMove(true);
        rb.velocity = Vector2.zero;
        Destroy(this.gameObject);
    }
}

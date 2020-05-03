using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController controller;
    private CombatControl combatController;

    private Vector2 pos;//Character position in Vector2

    [SerializeField] private GameObject arm;

    //Move
    private float inputX;
    private float inputY;
    [Range(0, 1)] [SerializeField] private float speed;

    //Mouse
    private Vector2 cursorPos; //Position of mouse

    //Shooting
    [Range(-180f, 180f)] public float offset; //angle offset of projectile

    void Start()
    {
        controller = GetComponent<CharacterController>();
        combatController = GetComponent<CombatControl>();
    }

    void Update()
    {
        pos = new Vector2(transform.position.x, transform.position.y);

        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 diff = cursorPos - pos;
        float shootAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            combatController.Fire();
        }
        Debug.Log(shootAngle);
        arm.transform.rotation = Quaternion.Euler(0, 0, shootAngle);
    }
    void FixedUpdate()
    {
        arm.transform.position = transform.position;
        Vector3 movement = new Vector3(inputX, inputY, 0f);
        controller.Move(movement * speed);
    }
}

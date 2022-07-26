using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;

    public float speed;

    Vector2 movement;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0)
        {
            transform.localScale = new Vector3(movement.x, 1, 1);
        }

        SwitchAnim();
    }

    private void FixedUpdate()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + movement * speed * Time.fixedDeltaTime);
    }

    void SwitchAnim()
    {
        animator.SetFloat("speed", movement.sqrMagnitude);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_movement : MonoBehaviour
{
    private Rigidbody2D rb2D;
   // private Animator animate;

    private float moveSpeed;
    private float jumpForce;
    private bool isJumping;
    private bool facingRight = true;

    private InputAction movementInputAction;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    //    animate = gameObject.GetComponent<Animator>();

        moveSpeed = 9.5f;
        jumpForce = 100f;
        isJumping = false;
        facingRight = true;

        movementInputAction = new InputAction("move", InputActionType.Value, "<Keyboard>/W, A, S, D");
        movementInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = movementInputAction.ReadValue<Vector2>();
        //animate.SetFloat("Speed", Mathf.Abs(moveInput.x));

        if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }
        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        Vector2 moveInput = movementInputAction.ReadValue<Vector2>();

        if (moveInput.x > 0.1f || moveInput.x < -0.1f)
        {
            rb2D.AddForce(new Vector2(moveInput.x * moveSpeed, 0f), ForceMode2D.Impulse);
        }

        if (!isJumping && moveInput.y > 0.1f)
        {
            rb2D.AddForce(new Vector2(0f, moveInput.y * jumpForce), ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}

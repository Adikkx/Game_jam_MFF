using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    public int Level;
    private Rigidbody2D body;
    public Text LevelText;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private Animator animator;
    public bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        LevelText = GameObject.Find("LevelText").GetComponent<Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        LevelText.text=""+Leveling.Level;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);
                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));

                    if (!success)
                    {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }

                animator.SetBool("IsMoving", success);
                animator.SetFloat("MoveX", movementInput.x);
                animator.SetFloat("MoveY", movementInput.y);
                if (movementInput.x < 0) spriteRenderer.flipX = true;
                else if (movementInput.x > 0) spriteRenderer.flipX = false;
            }
            else
            {
                animator.SetBool("IsMoving", false);
                if (animator.GetFloat("MoveX") < 0) spriteRenderer.flipX = true;
                else if (animator.GetFloat("MoveY") > 0) spriteRenderer.flipX = false;
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction == Vector2.zero) return false;
        int count = body.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
        if (count == 0)
        {
            body.MovePosition(body.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    private void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();

    }


    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Door")
        {
            Leveling.Level +=1;
            LevelText.text=""+Leveling.Level;
        } 
    }
}

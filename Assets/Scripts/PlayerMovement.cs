using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _jumpSpeed = 5f;

    private Vector2 _moveInput;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    CapsuleCollider2D _capsuleCollider2D;

    public void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<Vector2>();
    }

    public void OnJump(InputValue inputValue)
    {
        if (!_capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; } //todo узнать за логику

        if (inputValue.isPressed)
        {
            _rigidbody2D.velocity += new Vector2(0f, _jumpSpeed);
        }
    }

    private void Start()
    {
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Run();
        FlipSprite();
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x), 1f);
        }
    }

    private void Run()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
        Vector2 playerVelocity = new Vector2(_moveInput.x * _runSpeed, _rigidbody2D.velocity.y);

        _rigidbody2D.velocity = playerVelocity;

        _animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }
}

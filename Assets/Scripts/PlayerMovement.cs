using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _jumpSpeed = 5f;
    [SerializeField] private float _climbSpeed = 5f;
    private float _gravityScaleAtStart;

    private Vector2 _moveInput;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private CapsuleCollider2D _bodyCollider2D;
    private BoxCollider2D _feetCollider2D;

    public void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<Vector2>();
    }

    public void OnJump(InputValue inputValue)
    {
        if (!_feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (inputValue.isPressed)
        {
            _rigidbody2D.velocity += new Vector2(0f, _jumpSpeed);
        }
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bodyCollider2D = GetComponent<CapsuleCollider2D>();
        _feetCollider2D = GetComponent<BoxCollider2D>();

        _gravityScaleAtStart = _rigidbody2D.gravityScale;
    }

    private void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    private void ClimbLadder()
    {
        if (!_feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            _rigidbody2D.gravityScale = _gravityScaleAtStart;
            _animator.SetBool("IsClimbing", false);

            return;
        }

        Vector2 climbVelocity = new Vector2(_rigidbody2D.velocity.x, _moveInput.y * _climbSpeed);
        _rigidbody2D.velocity = climbVelocity;
        _rigidbody2D.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(_rigidbody2D.velocity.y) > Mathf.Epsilon;

        _animator.SetBool("IsClimbing", playerHasVerticalSpeed);
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
        Vector2 playerVelocity = new Vector2(_moveInput.x * _runSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
        _animator.SetBool("IsRunning", playerHasHorizontalSpeed);
    }
}
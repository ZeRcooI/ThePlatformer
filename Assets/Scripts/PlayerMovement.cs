using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(BoxCollider2D))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed Control")]
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _jumpSpeed = 5f;
    [SerializeField] private float _climbSpeed = 5f;

    [SerializeField] private Vector2 _deathKick = new Vector2(0f, 5f);
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _gun;

    private float _gravityScaleAtStart;

    private string _layerGround = "Ground";
    private string _layerHazard = "Hazards";
    private string _layerEnemies = "Enemies";
    private string _layerClimbing = "Climbing";
    private string _animationRunning = "IsRunning";
    private string _animationDying = "Dying";
    private string _animationClimbing = "IsClimbing";

    private Vector2 _moveInput;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    //private CapsuleCollider2D _bodyCollider2D;
    private BoxCollider2D _feetCollider2D;

    private bool _isAlive = true;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        //_bodyCollider2D = GetComponent<CapsuleCollider2D>();
        _feetCollider2D = GetComponent<BoxCollider2D>();

        _gravityScaleAtStart = _rigidbody2D.gravityScale;
    }

    private void Update()
    {
        if (_isAlive == false)
            return;

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    public void OnFire(InputValue inputValue)
    {
        if (_isAlive == false)
            return;

        Instantiate(_bullet, _gun.position,transform.rotation);
    }

    public void OnMove(InputValue inputValue)
    {
        if (_isAlive == false)
            return;

        _moveInput = inputValue.Get<Vector2>();
    }

    public void OnJump(InputValue inputValue)
    {
        if (_isAlive == false)
            return;

        if (!_feetCollider2D.IsTouchingLayers(LayerMask.GetMask(_layerGround)))
            return;

        if (inputValue.isPressed)
            _rigidbody2D.velocity += new Vector2(0f, _jumpSpeed);
    }

    private void Die()
    {
        if (_rigidbody2D.IsTouchingLayers(LayerMask.GetMask(_layerEnemies, _layerHazard)))
        {
            _isAlive = false;

            _animator.SetTrigger(_animationDying);

            _rigidbody2D.velocity = _deathKick;
        }
    }

    private void ClimbLadder()
    {
        if (!_feetCollider2D.IsTouchingLayers(LayerMask.GetMask(_layerClimbing)))
        {
            _rigidbody2D.gravityScale = _gravityScaleAtStart;
            _animator.SetBool(_animationClimbing, false);

            return;
        }

        Vector2 climbVelocity = new Vector2(_rigidbody2D.velocity.x, _moveInput.y * _climbSpeed);
        _rigidbody2D.velocity = climbVelocity;
        _rigidbody2D.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(_rigidbody2D.velocity.y) > Mathf.Epsilon;

        _animator.SetBool(_animationClimbing, playerHasVerticalSpeed);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x), 1f);
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveInput.x * _runSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
        _animator.SetBool(_animationRunning, playerHasHorizontalSpeed);
    }
}
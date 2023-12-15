using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bullsetSpeed = 5f;

    private float _xSpeed;
    private string _tagEnemy = "Enemy";

    private Rigidbody2D _rigidbody2D;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerMovement = FindObjectOfType<PlayerMovement>();

        _xSpeed = _playerMovement.transform.localScale.x * _bullsetSpeed;
    }

    private void Update()
    {
        _rigidbody2D.velocity = new Vector2(_xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_tagEnemy))
        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
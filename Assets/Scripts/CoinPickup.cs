using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    PlayerMovement _player;

    [SerializeField] private AudioClip _coinPickupSfx;

    private string _playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            AudioSource.PlayClipAtPoint(_coinPickupSfx, Camera.main.transform.position);

            Destroy(gameObject);
        }
    }
}
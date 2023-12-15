using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip _coinPickupSfx;
    [SerializeField] private int _pointsForCoinPickup = 100;

    bool _wasCollected = false;

    private string _playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag) && !_wasCollected)
        {
            _wasCollected = true;

            FindObjectOfType<GameSession>().AddScore(_pointsForCoinPickup);

            AudioSource.PlayClipAtPoint(_coinPickupSfx, Camera.main.transform.position);

            Destroy(gameObject);
        }
    }
}
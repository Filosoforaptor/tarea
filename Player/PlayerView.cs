using UnityEngine;
using UnityEngine.UI; // Still needed if you use other UI elements, though TextMeshPro is separate
using TMPro;

public class PlayerView : MonoBehaviour
{
    private PlayerPresenter playerPresenter;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI healthText; // For displaying player health

    [Header("Effects")]
    [SerializeField] private AudioSource coinSound;
    [SerializeField] private ParticleSystem _particleSystem;
    // Optional: [SerializeField] private AudioSource damageSound;
    // Optional: [SerializeField] private AudioSource deathSound;

    private void Awake()
    {
        playerPresenter = GetComponent<PlayerPresenter>();
        if (playerPresenter == null)
        {
            Debug.LogError("PlayerPresenter not found on this GameObject!", this);
        }
    }

    private void OnEnable()
    {
        if (playerPresenter != null)
        {
            playerPresenter.OnCoinsCollected += HandleCoinsCollected;
            playerPresenter.OnPlayerMoving += HandlePlayerMoving;
            playerPresenter.OnPlayerHealthChangedUI += HandlePlayerHealthChanged; // Subscribe to health changes
            playerPresenter.OnPlayerDiedUI += HandlePlayerDied; // Subscribe to player death
        }
    }

    private void OnDisable()
    {
        if (playerPresenter != null)
        {
            playerPresenter.OnCoinsCollected -= HandleCoinsCollected;
            playerPresenter.OnPlayerMoving -= HandlePlayerMoving;
            playerPresenter.OnPlayerHealthChangedUI -= HandlePlayerHealthChanged; // Unsubscribe
            playerPresenter.OnPlayerDiedUI -= HandlePlayerDied; // Unsubscribe
        }
    }

    private void HandlePlayerMoving(bool isMoving)
    {
        if (isMoving)
        {
            if (!_particleSystem.isPlaying)
            {
                _particleSystem.Play();
            }
        }
        else
        {
            if (_particleSystem.isPlaying)
            {
                _particleSystem.Stop();
            }
        }
    }

    private void HandleCoinsCollected(int newCoinCount)
    {
        if (coinSound != null)
            coinSound.Play();

        if (coinsText != null)
            coinsText.text = "Coins: " + newCoinCount;
    }

    private void HandlePlayerHealthChanged(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
        }
        // Optional: Play damage sound if currentHealth < previousHealth
        // if (damageSound != null && currentHealth < /* store previous health to check */)
        //    damageSound.Play();
        Debug.Log($"Player health updated: {currentHealth}/{maxHealth}");
    }

    private void HandlePlayerDied()
    {
        Debug.Log("PlayerView: Player has died! Game Over sequence should start.");
        // Optional: Play death sound
        // if (deathSound != null)
        //    deathSound.Play();

        // In the next step, we will activate the Game Over UI here.
    }
}
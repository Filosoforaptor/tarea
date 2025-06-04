using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerView : MonoBehaviour
{
    private PlayerPresenter playerPresenter;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private AudioSource coinSound;
    [SerializeField] private ParticleSystem _particleSystem;

    private void Awake()
    {
        playerPresenter = GetComponent<PlayerPresenter>();
    }
    private void OnEnable()
    {
        if (playerPresenter != null)
        {
            playerPresenter.OnCoinsCollected += HandleCoinsCollected;
            playerPresenter.OnPlayerMoving += HandlePlayerMoving;
        }
      

    }

    private void OnDisable()
    {
        if (playerPresenter != null)
        {
            playerPresenter.OnCoinsCollected -= HandleCoinsCollected;
            playerPresenter.OnPlayerMoving -= HandlePlayerMoving;
        }
          
    }
    private void HandlePlayerMoving(bool value)
    {
        if (value)
        {
            _particleSystem.Play();
        }
        else
        {
            _particleSystem.Stop();
        }
       
    }
    private void HandleCoinsCollected(int newCoinCount)
    {

        // Reproduce sonido si está asignado
        if (coinSound != null)
            coinSound.Play();
 
        // Actualiza el texto con la nueva cantidad de monedas
        if (coinsText != null)
            coinsText.text = "Coins: " + newCoinCount;

    }
}

using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerPresenter : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerModel _model;
    private PlayerInput _pInput;

    [SerializeField]
    private GameObject _mesh;
    public Action<bool> OnPlayerMoving { get; set; }
    public Action<int> OnCoinsCollected { get; set; }
    public Action<int, int> OnPlayerHealthChangedUI { get; set; } // For the view
    public Action OnPlayerDiedUI { get; set; }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _model = GetComponent<PlayerModel>();
        _pInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _model.OnCoinsChanged += PresenterCoinsChanged;
        // Subscribe to health events from the model
        if (_model != null) // Good practice to check if model exists
        {
            _model.OnHealthChanged += PresenterHealthChanged;
            _model.OnPlayerDied += PresenterPlayerDied;
        }
    }

    private void OnDisable()
    {
        _model.OnCoinsChanged -= PresenterCoinsChanged;
        // Unsubscribe from health events
        if (_model != null) // Good practice to check if model exists
        {
            _model.OnHealthChanged -= PresenterHealthChanged;
            _model.OnPlayerDied -= PresenterPlayerDied;
        }
    }

    void FixedUpdate()
    {
        Vector3 input = _pInput.Axis;
        ApplyMovement(input);
        UpdateTilt(input.x);
    }

    public void ApplyMovement(Vector3 direction)
    {
        _rb.velocity = _model.CalculateMove(direction);

        bool isMoving = direction.magnitude > 0.1f;
        OnPlayerMoving?.Invoke(isMoving);
    }

    // Metodo para actualizar la inclinación del mesh
    private void UpdateTilt(float inputX)
    {
        Quaternion targetRotation = _model.CalculateTargetRotation(inputX);
        Quaternion currentRotation = _mesh.transform.localRotation;
        _mesh.transform.localRotation = Quaternion.Slerp(currentRotation, targetRotation, _model.TiltSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            _model.AddCoin();
            Destroy(other.gameObject);
        }
        // Note: Meteorite collision is handled by Meteorite.cs calling ProcessDamage on this presenter.
    }

    private void PresenterCoinsChanged(int newCoinCount)
    {
        OnCoinsCollected?.Invoke(newCoinCount);
    }

    // Method called by Meteorite to inflict damage
    public void ProcessDamage(int damageAmount)
    {
        if (_model != null)
        {
            _model.TakeDamage(damageAmount);
        }
    }

    // Handler for the model's OnHealthChanged event
    private void PresenterHealthChanged(int currentHealth, int maxHealth)
    {
        OnPlayerHealthChangedUI?.Invoke(currentHealth, maxHealth);
    }

    // Handler for the model's OnPlayerDied event
    private void PresenterPlayerDied()
    {
        Debug.Log("Player has died! (Presenter)");
        OnPlayerDiedUI?.Invoke();
        if (_pInput != null)
        {
            _pInput.enabled = false; // Disable the PlayerInput component
        }
        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
            _rb.isKinematic = true; // Makes it ignore physics forces, good for a "dead" state
        }
        // Consider disabling this component or the entire GameObject if appropriate
        // this.enabled = false; 
    }
}
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    public int damageAmount = 1; // Amount of damage this meteorite inflicts

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPresenter playerPresenter = other.GetComponent<PlayerPresenter>();
            if (playerPresenter != null)
            {
                playerPresenter.ProcessDamage(damageAmount);
            }
            else
            {
                Debug.LogError("Meteorite collided with Player, but PlayerPresenter component not found on the player object.");
            }
            // Destroy the meteorite after hitting the player
            Destroy(gameObject);
        }
        // Note: Off-screen destruction/recycling is expected to be handled by Respawner.cs
        // if this meteorite also has the "Meteorite" tag and hits the Respawner's trigger.
    }

    // It's good practice to ensure the meteorite has a Rigidbody if it's expected to move
    // and a Collider (set to trigger) to detect collisions.
    // These would typically be added in the Unity Editor on the meteorite prefab.
    // We'll assume the meteorite prefab will also be tagged "Meteorite" for the Respawner.
}
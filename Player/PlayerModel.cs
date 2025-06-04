using System;
using UnityEngine;
public class PlayerModel : MonoBehaviour
{
    [SerializeField]
    private Quaternion initTiltRotation;

    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float tiltAngle = 30f; // cuanto inclina la nave

    [SerializeField]
    private float tiltSpeed = 5f;  // que tan rapido se inclina

    public int CurrentCoins { get; private set; }
    public float TiltSpeed { get => tiltSpeed; private set => tiltSpeed = value; }

    public event Action<int> OnCoinsChanged;

  
    public Vector3 CalculateMove(Vector3 direction)
    {
        return direction.normalized * moveSpeed;
    }
    public Quaternion CalculateTargetRotation(float inputX)
    {
        float tiltZ = 0f;
        if (Mathf.Abs(inputX) > 0.01f)
            tiltZ = -inputX * tiltAngle;

        return initTiltRotation * Quaternion.Euler(0f, 0f, tiltZ);
    }


    public void AddCoin()
    {
        CurrentCoins++;
        print("add coin model");
        OnCoinsChanged?.Invoke(CurrentCoins);
    }
}
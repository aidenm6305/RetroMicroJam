using UnityEngine;

/// <summary>
/// Controls the activation of TrailRenderers based on the car's lateral drift.
/// </summary>
[RequireComponent(typeof(CarMovement), typeof(Rigidbody2D))]
public class CarSkidController : MonoBehaviour
{
    [Header("Visuals")]
    [Tooltip("Assign the TrailRenderers attached to the wheel positions.")]
    public TrailRenderer[] skidTrails;

    [Header("Drift Settings")]
    [Tooltip("Must match the threshold in CarAudioController for synced AV feedback.")]
    public float driftSlipThreshold = 6.0f;
    public float minSpeedToDrift = 8.0f;

    CarMovement carMovement;
    Rigidbody2D rigidbody2D;

    void Awake()
    {
        carMovement = GetComponent<CarMovement>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        StopSkidding();
    }

    void Update()
    {
        HandleSkidMarks();
    }

    void HandleSkidMarks()
    {
        if (skidTrails.Length == 0) return;

        float lateralSpeed = Mathf.Abs(Vector2.Dot(rigidbody2D.linearVelocity, transform.right));
        float forwardSpeed = Mathf.Abs(carMovement.currentSpeed);

        if (forwardSpeed > minSpeedToDrift && lateralSpeed > driftSlipThreshold)
        {
            StartSkidding();
        }
        else
        {
            StopSkidding();
        }
    }

    void StartSkidding()
    {
        foreach (TrailRenderer trail in skidTrails)
        {
            if (trail != null) trail.emitting = true;
        }
    }

    void StopSkidding()
    {
        foreach (TrailRenderer trail in skidTrails)
        {
            if (trail != null) trail.emitting = false;
        }
    }
}
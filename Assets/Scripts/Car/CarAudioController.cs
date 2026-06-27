using UnityEngine;

/// <summary>
/// Handles all audio feedback for the car, reading physical states from CarMovement.
/// Requires AudioSources for the engine, drifting, and boosting.
/// </summary>
[RequireComponent(typeof(CarMovement), typeof(Rigidbody2D))]
public class CarAudioController : MonoBehaviour
{
    [Header("Audio Sources")]
    [Tooltip("Assign an AudioSource that loops an engine idle/driving clip.")]
    public AudioSource engineAudio;
    [Tooltip("Assign an AudioSource that loops a tire screech clip.")]
    public AudioSource driftAudio;
    [Tooltip("Assign an AudioSource with a boost sound effect.")]
    public AudioSource boostAudio;

    [Header("Engine Settings")]
    public float minPitch = 0.8f;
    public float maxPitch = 2.5f;
    public float pitchMultiplier = 1.5f;

    [Header("Drift Settings")]
    [Tooltip("How much lateral (sideways) velocity is required to start playing the drift sound.")]
    public float driftSlipThreshold = 2.5f;

    CarMovement carMovement;
    Rigidbody2D rigidbody2D;
    bool wasBoostingLastFrame = false;

    void Awake()
    {
        carMovement = GetComponent<CarMovement>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // Ensure our looping sounds are actually set to loop
        if (engineAudio != null) engineAudio.loop = true;
        if (driftAudio != null) driftAudio.loop = true;
    }

    void Update()
    {
        HandleEngineAudio();
        HandleDriftAudio();
        HandleBoostAudio();
    }

    void HandleEngineAudio()
    {
        if (engineAudio == null) return;

        // Calculate a pitch value based on the car's current forward speed vs its normal max speed
        // We use absolute value so reversing (if added later) also revs the engine
        float speedRatio = Mathf.Abs(carMovement.currentSpeed) / 20f; // 20f is your base maxSpeed

        float targetPitch = minPitch + (speedRatio * pitchMultiplier);

        // Clamp it so a massive boost doesn't make the audio sound like a mosquito
        engineAudio.pitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);
    }

    void HandleDriftAudio()
    {
        if (driftAudio == null) return;

        // Calculate how fast the car is sliding sideways using the Dot Product
        float lateralSpeed = Mathf.Abs(Vector2.Dot(rigidbody2D.linearVelocity, transform.right));

        // If we are sliding faster than the threshold, fade the sound in. Otherwise, fade it out.
        if (lateralSpeed > driftSlipThreshold)
        {
            // Optional: Map the volume to how hard we are drifting (clamped between 0 and 1)
            driftAudio.volume = Mathf.Lerp(driftAudio.volume, .25f, Time.deltaTime * 10f);
        }
        else
        {
            driftAudio.volume = Mathf.Lerp(driftAudio.volume, 0f, Time.deltaTime * 10f);
        }
    }

    void HandleBoostAudio()
    {
        if (boostAudio == null) return;

        // Detect the exact frame the boost starts so we only play the initial sound once
        if (carMovement.IsBoosting && !wasBoostingLastFrame)
        {
            boostAudio.Play();
        }
        // Optional: Stop the boost sound if they let go early
        else if (!carMovement.IsBoosting && wasBoostingLastFrame)
        {
            boostAudio.Stop();
        }

        wasBoostingLastFrame = carMovement.IsBoosting;
    }
}
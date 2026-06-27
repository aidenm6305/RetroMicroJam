using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class is responsible for handling the movement of the car in the game.
/// It will handle user input and apply forces to the car's Rigidbody component to move it 
/// around the scene. The car will always move forward and only have left right and handbrake
/// no stopping can be controlled by the player
/// </summary>
public class CarMovement : MonoBehaviour
{
    [Header("Car Settings")]
    public float driftFactor = 0.95f;
    [SerializeField]
    float accelerationFactor = 30f;
    public float maxSpeed = 20f;
    private float maxStartSpeed = 20f;
    public float turnfactor = 3.5f;

    [Tooltip("How quickly the car slows down to normal speed after boosting.")]
    public float boostDeceleration = 2f;

    public float currentSpeed = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    bool isBoosting = false;
    Rigidbody2D rigidbody2D;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rotationAngle = rigidbody2D.rotation;
    }

    private void Start()
    {
        maxStartSpeed = maxSpeed;
    }

    void FixedUpdate()
    {
        ApplyEngineForce();
        ApplySteering();
        KillOrthogonalVelocity();
    }

    void ApplyEngineForce()
    {
        currentSpeed = Vector2.Dot(rigidbody2D.linearVelocity, transform.up);

        if (!isBoosting && currentSpeed > maxSpeed)
        {
            Vector2 forwardVelocity = transform.up * currentSpeed;
            Vector2 rightVelocity = transform.right * Vector2.Dot(rigidbody2D.linearVelocity, transform.right);

            float deceleratedSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.fixedDeltaTime * boostDeceleration);

            rigidbody2D.linearVelocity = (Vector2)transform.up * deceleratedSpeed + rightVelocity;

            return;
        }

        if (currentSpeed <= maxSpeed)
        {
            Vector2 engineForceVector = transform.up * accelerationFactor;
            rigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
        }
    }

    void ApplySteering()
    {
        float minSpeedBeforeAllowTurningFactor = (rigidbody2D.linearVelocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);
        rotationAngle -= steeringInput * turnfactor * minSpeedBeforeAllowTurningFactor;
        rigidbody2D.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rigidbody2D.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rigidbody2D.linearVelocity, transform.right);
        rigidbody2D.linearVelocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
    }

    void OnSprint(InputValue value)
    {
        float maxBoostSpeed = 30f;
        isBoosting = value.isPressed;
        maxSpeed = isBoosting ? maxBoostSpeed : maxStartSpeed;
    }
}
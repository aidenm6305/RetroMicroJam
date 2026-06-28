using UnityEngine;

public class BoostAnimation : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the parent car object here to read its boost state.")]
    [SerializeField] private CarMovement carMovement;

    [Header("Animation Settings")]
    [Tooltip("How fast the sprite flips back and forth (in seconds).")]
    [SerializeField] private float animationSpeed = 0.05f;

    private SpriteRenderer spriteRenderer;
    private float timer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        carMovement = GetComponentInParent<CarMovement>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }

    private void Update()
    {
        if (carMovement == null || spriteRenderer == null) return;

        if (carMovement.IsBoosting)
        {
            spriteRenderer.enabled = true;

            timer += Time.deltaTime;
            if (timer >= animationSpeed)
            {
                timer = 0f;

                Vector3 currentScale = transform.localScale;
                currentScale.x *= -1;
                transform.localScale = currentScale;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
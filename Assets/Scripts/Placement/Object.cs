using UnityEngine;

public enum HazardType { Solid, Oil, Mud, Fire }

public class Object : MonoBehaviour
{
    [Header("Settings")]
    public HazardType hazardType = HazardType.Solid;

    private bool canBePlaced = false;
    private bool isPlaced = false;

    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true; 
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (isPlaced) return;

        int overlap = circleCollider.Overlap(new ContactFilter2D(), new Collider2D[10]);
        canBePlaced = overlap == 0;
        spriteRenderer.color = canBePlaced ? Color.green : Color.red;
    }

    public bool PlaceObject()
    {
        if (canBePlaced)
        {
            isPlaced = true;
            spriteRenderer.color = Color.white;

            if (hazardType == HazardType.Solid)
            {
                circleCollider.isTrigger = false;
            }

            return true;
        }
        return false;
    }

    public void SetLocation(Vector3 setPosition)
    {
        if (isPlaced) return;
        transform.position = setPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlaced) return;

        if (collision.CompareTag("Player"))
        {
            CarMovement car = collision.GetComponent<CarMovement>();
            if (car != null)
            {
                if (hazardType == HazardType.Oil) car.ApplyOilEffect();
                else if (hazardType == HazardType.Mud) car.ApplyMudEffect();
                else if (hazardType == HazardType.Fire) car.ApplyFireEffect();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isPlaced) return;

        if (collision.CompareTag("Player"))
        {
            CarMovement car = collision.GetComponent<CarMovement>();
            if (car != null)
            {
                if (hazardType == HazardType.Oil) car.RemoveEffects();
                else if (hazardType == HazardType.Mud) car.RemoveEffects();
            }
        }
    }
}
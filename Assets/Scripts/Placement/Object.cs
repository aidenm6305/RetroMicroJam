using UnityEngine;

public class Object : MonoBehaviour
{
    private bool canBePlaced = false;
    private bool isPlaced = false;
    [SerializeField]
    private bool noCollision = false;
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
        Debug.Log("Update called. isPlaced: " + isPlaced + ", canBePlaced: " + canBePlaced);
        if (isPlaced) return;

        int overlap = circleCollider.Overlap(new ContactFilter2D(), new Collider2D[10]);
        Debug.Log("Overlap count: " + overlap);
        canBePlaced = overlap == 0;
        spriteRenderer.color = canBePlaced ? Color.green : Color.red;
    }

    public bool PlaceObject()
    {
        if (canBePlaced)
        {
            isPlaced = true;
            if (!noCollision)
                circleCollider.isTrigger = false;
            spriteRenderer.color = Color.white;
            Debug.Log("Object placed successfully.");
            return true;
        }
        Debug.Log("Failed to place object.");
        return false;
    }

    public void SetLocation(Vector3 setPosition)
    {
        if (isPlaced) return;
        transform.position = setPosition;
        Debug.Log(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (noCollision)
        {
            //apply some sliding thing
        }
    }
}
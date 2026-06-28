using UnityEngine;
using System.Collections.Generic;

public class Place : MonoBehaviour
{

    [SerializeField] 
    private List<Object> objectsToPlace;

    [SerializeField]
    private GameManager gameManager;
    private bool isPlacing = false;
    private Object currentObject;

    public void SetPlacement()
    {
        Debug.Log("SetPlacement called. Starting placement of a new object.");
        Cursor.visible = true;
        currentObject = objectsToPlace[Random.Range(0, objectsToPlace.Count)];
        isPlacing = true;
        currentObject = Instantiate(currentObject, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if (!isPlacing) return;

        if (Input.GetMouseButtonDown(0))
        {
            bool didPlace = currentObject.PlaceObject();
            if (didPlace)
            {
                isPlacing = false;
                Cursor.visible = false;
                gameManager.ObjectPlaced(); 
            }
            return;
        }
        currentObject.SetLocation(Camera.main.ScreenToWorldPoint(Input.mousePosition));

    }
}
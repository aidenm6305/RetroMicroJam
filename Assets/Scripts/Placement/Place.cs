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
    private List<GameObject> activeObstacles = new List<GameObject>();

    public void SetPlacement()
    {
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
                activeObstacles.Add(currentObject.gameObject);

                isPlacing = false;
                Cursor.visible = false;
                gameManager.ObjectPlaced();
            }
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 
        currentObject.SetLocation(mousePos);
    }

    public void ClearAllObstacles()
    {
        foreach (GameObject obs in activeObstacles)
        {
            if (obs != null)
            {
                Destroy(obs);
            }
        }
        activeObstacles.Clear(); 
    }
}
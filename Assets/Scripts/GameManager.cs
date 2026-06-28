using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TrackManager trackManager;
    [SerializeField] private CarMovement playerCar;
    [SerializeField] private GameObject obstacleSelectionUI;

    [Header("Spawn Settings")]
    [SerializeField] private Transform startPoint;

    [SerializeField] private GameObject fullCamera;
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private Place placeController;
    public void ObjectPlaced()
    {
        fullCamera.SetActive(false);
        mainCamera.SetActive(true);

        if (playerCar != null && startPoint != null)
        {
            playerCar.ResetCar(startPoint.position, startPoint.rotation);
        }

        Time.timeScale = 1f;
    }

    private void Start()
    {
        obstacleSelectionUI.SetActive(false);
        Time.timeScale = 1f;

        if (trackManager != null)
        {
            trackManager.OnLapCompleted += HandleLapCompleted;
        }
    }

    private void OnDestroy()
    {
        if (trackManager != null)
        {
            trackManager.OnLapCompleted -= HandleLapCompleted;
        }
    }

    private void HandleLapCompleted()
    {
        Time.timeScale = 0f;

        obstacleSelectionUI.SetActive(true);

    }

    public void OnObstacleSelected()
    {
        obstacleSelectionUI.SetActive(false);
        fullCamera.SetActive(true);
        mainCamera.SetActive(false);

        if (placeController != null)
        {
            placeController.SetPlacement();
        }
    }
}
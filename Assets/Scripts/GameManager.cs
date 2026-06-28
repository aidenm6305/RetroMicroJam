using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TrackManager trackManager;
    [SerializeField] private CarMovement playerCar;
    [SerializeField] private GameObject obstacleSelectionUI;

    [Header("Spawn Settings")]
    [SerializeField] private Transform startPoint;
    [SerializedField] private  
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

        if (playerCar != null && startPoint != null)
        {
            playerCar.ResetCar(startPoint.position, startPoint.rotation);
        }

        Time.timeScale = 1f;
    }
}
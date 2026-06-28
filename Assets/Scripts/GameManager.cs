using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TrackManager trackManager;
    [SerializeField] private CarMovement playerCar;
    [SerializeField] private GameObject obstacleSelectionUI;
    [SerializeField] private TextMeshProUGUI obstacleLapText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private GameObject fullCamera;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Place placeController;

    [Header("Game Over UI")]
    [SerializeField] private GameObject loseScreenUI;
    [SerializeField] private TextMeshProUGUI loseScoreText;

    [Header("Spawn Settings")]
    [SerializeField] private Transform startPoint;

    [Header("Lap & Score Tracking")]
    public int currentLap = 1;
    public int totalScore = 0;
    public float currentLapTime = 0f;

    [Tooltip("The maximum points awarded for a perfect lap time of 0 seconds.")]
    [SerializeField] private int baseLapScore = 1000;

    [Tooltip("How many points are lost per second it takes to complete the lap.")]
    [SerializeField] private int scorePenaltyPerSecond = 20;

    [Header("Stuck Reset Settings")]
    [Tooltip("How long the player must be stopped before resetting.")]
    [SerializeField] private float stuckTimeLimit = 2f;
    [Tooltip("Speeds below this value are considered 'stuck' (accounts for physics drift).")]
    [SerializeField] private float speedThreshold = 0.5f;
    private float stuckTimer = 0f;

    private bool isRacing = false;

    private void Start()
    {
        obstacleSelectionUI.SetActive(false);
        Time.timeScale = 1f;
        isRacing = true;

        if (trackManager != null)
        {
            trackManager.OnLapCompleted += HandleLapCompleted;
        }
        UpdateHUD();
        Cursor.visible = false;
    }

    private void UpdateHUD()
    {
        if (lapText != null)
        {
            lapText.SetText($"Lap: {currentLap}");
        }

        if (scoreText != null)
        {
            scoreText.SetText($"Score: {totalScore}");
        }
    }

    private void Update()
    {
        if (isRacing)
        {
            currentLapTime += Time.deltaTime;

            if (playerCar != null)
            {
                if (Mathf.Abs(playerCar.currentSpeed) <= speedThreshold)
                {
                    stuckTimer += Time.deltaTime;

                    if (stuckTimer >= stuckTimeLimit)
                    {
                        TriggerGameOver();
                    }
                }
                else
                {
                    stuckTimer = 0f;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (trackManager != null)
        {
            trackManager.OnLapCompleted -= HandleLapCompleted;
        }
    }

    public void TriggerGameOver()
    {
        Debug.Log("Player was stuck! Game Over.");
        
        Cursor.visible = true;
        isRacing = false;
        Time.timeScale = 0f;

        if (loseScreenUI != null) loseScreenUI.SetActive(true);
        if (loseScoreText != null) loseScoreText.SetText($"Game Over!\nFinal Score: {totalScore}\nCompleted Laps: {currentLap-1}");
    }

    public void RestartGame()
    {
        totalScore = 0;
        currentLap = 1;
        currentLapTime = 0f;
        stuckTimer = 0f;

        if (loseScreenUI != null) loseScreenUI.SetActive(false);

        if (playerCar != null && startPoint != null)
        {
            playerCar.ResetCar(startPoint.position, startPoint.rotation);
        }
        if (placeController != null)
            placeController.ClearAllObstacles();

        trackManager.ResetCheckpoints();
        UpdateHUD();
        isRacing = true;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    private void HandleLapCompleted()
    {
        Time.timeScale = 0f;
        isRacing = false;

        CalculateLapScore();

        currentLap++;
        Cursor.visible = true;

        obstacleSelectionUI.SetActive(true);
    }

    private void CalculateLapScore()
    {
        int timePenalty = Mathf.RoundToInt(currentLapTime * scorePenaltyPerSecond);

        //Ensure pointsEarned doesn't drop below 0
        int pointsEarned = Mathf.Max(0, baseLapScore - timePenalty);

        totalScore += pointsEarned;


        scoreText.text = $"Score: {totalScore}";
        lapText.text = $"Lap: {currentLap}";
        obstacleLapText.text = $"Lap {currentLap} completed! Time: {currentLapTime:F2}s, Points Earned: {pointsEarned}";
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

    public void ObjectPlaced()
    {
        fullCamera.SetActive(false);
        mainCamera.SetActive(true);

        if (playerCar != null && startPoint != null)
        {
            playerCar.ResetCar(startPoint.position, startPoint.rotation);
        }

        currentLapTime = 0f;
        isRacing = true;
        Time.timeScale = 1f;
    }
}
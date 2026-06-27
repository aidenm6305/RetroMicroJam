using TMPro;
using UnityEngine;

public class CarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    CarMovement carMovement;

    [SerializeField]
    TMP_Text speedometerText;

    [SerializeField]
    RectTransform speedNeedle;

    [Header("Needle Settings")]
    [SerializeField]
    float minNeedleAngle = -120f;

    [SerializeField]
    float maxNeedleAngle = 120f;

    [SerializeField]
    float maxDisplayedSpeed = 11f;

    void Awake()
    {
        if (carMovement == null)
        {
            carMovement = GetComponent<CarMovement>();
        }
    }

    void Update()
    {
        UpdateSpeedometer();
        UpdateNeedle();
    }

    public void UpdateSpeedometer()
    {
        if (carMovement == null || speedometerText == null)
        {
            return;
        }

        speedometerText.SetText("{0:0} mph", carMovement.currentSpeed * 2.237f);
    }

    public void UpdateNeedle()
    {
        if (carMovement == null || speedNeedle == null)
        {
            return;
        }

        float normalizedSpeed = Mathf.Clamp01(carMovement.currentSpeed / maxDisplayedSpeed);
        float needleAngle = Mathf.Lerp(minNeedleAngle, maxNeedleAngle, normalizedSpeed);
        speedNeedle.localRotation = Quaternion.Euler(0f, 0f, needleAngle);
    }
}

using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    CarMovement carMovement;

    void Awake()
    {
        carMovement = GetComponent<CarMovement>();
    }

    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        inputVector.x = Input.GetAxis("Horizontal");
        carMovement.SetInputVector(inputVector);  
    }
}

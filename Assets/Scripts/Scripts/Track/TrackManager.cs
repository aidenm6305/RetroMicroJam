using UnityEngine;
using System.Collections.Generic;
using System;
public class TrackManager : MonoBehaviour
{

    [SerializeField] 
    private List<CheckPoint> checkPoints; 
    
    CheckPoint nextCheckPoint;
    int totalCheckPoints;

    public event Action OnLapCompleted;

    public void Start()
    {
        Debug.Log("TrackManager constructor called.");
        Debug.Log("Number of checkpoints: " + checkPoints.Count);
        totalCheckPoints = checkPoints.Count;
        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].Setup(i, this);
        }
        nextCheckPoint = checkPoints[1];

    }

    public CheckPoint GetNextPoint(int currentIndex)
    {
        return checkPoints[(currentIndex + 1) % totalCheckPoints];
    }

   public void CheckPointPassed(CheckPoint checkPoint)
    {
        
        Debug.Log("Checkpoint passed: " + checkPoint.GetIndex());
        if (checkPoint != nextCheckPoint)
        {
            Debug.Log("Wrong checkpoint passed! Expected: " + nextCheckPoint.GetIndex() + ", but got: " + checkPoint.GetIndex());
            return;
        }

        nextCheckPoint = GetNextPoint(checkPoint.GetIndex());

        if (nextCheckPoint == checkPoints[1])
        {
            Debug.Log("Lap completed!");
            OnLapCompleted?.Invoke();
        }
    } 

}
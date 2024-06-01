using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public int currentScore = 0;

    private void OnEnable()
    {
        EventManager.OnAddPoints += HandleAddPoints;
    }

    private void OnDisable()
    {
        EventManager.OnAddPoints -= HandleAddPoints;
    }

    private void HandleAddPoints(int points)
    {
        currentScore += points;
        Debug.Log(currentScore);
    }
}

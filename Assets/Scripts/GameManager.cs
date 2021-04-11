using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int pointsOnLevel;

    public void AddPoints(int _points)
    {
        pointsOnLevel += _points;
    }

    public void LevelFailed()
    {
        Debug.Log("End");
        //okno restrtu -> restart poziomu
    }
}

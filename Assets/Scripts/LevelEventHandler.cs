using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventHandler : MonoBehaviour
{
    [SerializeField]
    private int pointsOnLevel;
    [SerializeField]
    private int pointsToComplete;
    [SerializeField]
    private int thisLevelInt;
    [SerializeField]
    private ScenesManager.Scenes nextScene;

    public void AddPoints(int _points)
    {
        pointsOnLevel += _points;
        if(pointsOnLevel >= pointsToComplete)
        {
            LevelCompleted();
        }
    }

    public void LevelFailed()
    {
        Debug.Log("End");
        //okno restrtu -> restart poziomu
        ScenesManager.SM.ReloadLevel();
    }

    public void LevelCompleted()
    {
        if(PlayerPrefs.GetInt("mostLevelCompleted", 0) < thisLevelInt)
        {
            PlayerPrefs.SetInt("mostLevelCompleted", thisLevelInt);
        }
        //okno congratsów -> guzik next -> kolejny level / podsumowanie gry
        ScenesManager.SM.LoadScene(nextScene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(ScenesManager.SM != null)
            {
                Debug.Log("in reload");
                ScenesManager.SM.ReloadLevel();
            }
            else
            {
                Debug.Log("Singleton ScenesMenagera jeszcze nie powsta³");
            }
        }
    }
}

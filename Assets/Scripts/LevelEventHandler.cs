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
    private GameManager.Scenes nextScene;
    private bool levelComplete = false;

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
        if(!levelComplete)
        {
            Debug.Log("End");
            //okno restrtu -> restart poziomu
            GameManager.GM.ReloadLevel();
        }
    }

    public void LevelCompleted()
    {
        levelComplete = true;
        if(PlayerPrefs.GetInt("mostLevelCompleted", 0) < thisLevelInt)
        {
            PlayerPrefs.SetInt("mostLevelCompleted", thisLevelInt);
            GameManager.GM.SoundManager.PlaySound(SoundTypes.LevelComplete);
            GameManager.GM.LoadScene(nextScene, 1.1f, "Next Level Unlocked!");
        }
        else
        {
            GameManager.GM.LoadScene(nextScene);
        }
        //okno congratsów -> guzik next -> kolejny level / podsumowanie gry
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(GameManager.GM != null)
            {
                Debug.Log("in reload");
                GameManager.GM.ReloadLevel();
            }
            else
            {
                Debug.Log("Singleton ScenesMenagera jeszcze nie powsta³");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelButton : MonoBehaviour
{
    public GameManager.ScenesTypes sceneToLoad;
    private int mostLevelCompleted = 0;
    private Button myButton;
    private int myLevelInt;
    [SerializeField]
    private GameObject myLockImage;
    [SerializeField]
    private GameObject myTickImage;

    private void Awake()
    {
        mostLevelCompleted = PlayerPrefs.GetInt("mostLevelCompleted", 0);

        myButton = gameObject.GetComponent<Button>();

        switch (sceneToLoad)
        {
            case GameManager.ScenesTypes.Level1:
                myLevelInt = 1;
                break;
            case GameManager.ScenesTypes.Level2:
                myLevelInt = 2;
                break;
            case GameManager.ScenesTypes.Level3:
                myLevelInt = 3;
                break;
            case GameManager.ScenesTypes.Level4:
                myLevelInt = 4;
                break;
            case GameManager.ScenesTypes.Level5:
                myLevelInt = 5;
                break;
            case GameManager.ScenesTypes.Level6:
                myLevelInt = 6;
                break;
            case GameManager.ScenesTypes.Level7:
                myLevelInt = 7;
                break;
            default:
                Debug.LogError("LoadLevelButton's sceneToLoad is invalid");
                break;
        }
        if (mostLevelCompleted < myLevelInt - 1)
        {
            myButton.interactable = false;
        }
        else if(mostLevelCompleted == myLevelInt - 1)
        {
            myLockImage.SetActive(false);
        }
        else
        {
            myLockImage.SetActive(false);
            myTickImage.SetActive(true);
        }
    }

    public void LoadlevelCall()
    {
        GameManager.GM.LoadScene(sceneToLoad);
        GameManager.GM.SoundManager.PlaySound(SoundTypes.ButtonClick);
    }
}

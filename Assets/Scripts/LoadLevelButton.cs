using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelButton : MonoBehaviour
{
    public ScenesManager.Scenes sceneToLoad;
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
            case ScenesManager.Scenes.Level1:
                myLevelInt = 1;
                break;
            case ScenesManager.Scenes.Level2:
                myLevelInt = 2;
                break;
            case ScenesManager.Scenes.Level3:
                myLevelInt = 3;
                break;
            case ScenesManager.Scenes.Level4:
                myLevelInt = 4;
                break;
            case ScenesManager.Scenes.Level5:
                myLevelInt = 5;
                break;
            case ScenesManager.Scenes.Level6:
                myLevelInt = 6;
                break;
            case ScenesManager.Scenes.Level7:
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
        ScenesManager.SM.LoadScene(sceneToLoad);
    }
}

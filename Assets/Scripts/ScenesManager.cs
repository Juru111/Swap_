using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    #region Singleton
    public static ScenesManager SM;
    void Awake()
    {
        if (SM != null)
        {
            Destroy(gameObject);
            return;
        } 
        else
        {
            SM = this;
        }
        DontDestroyOnLoad(this);
    }
    #endregion

    #region ScenesManagment
    [SerializeField]
    private Animator animator;

    public enum Scenes
    {
        NONE,
        Menu,
        SampleScene,
        NONE2,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentScene)
            {
                case Scenes.Menu:
                    Debug.Log("Quitnig...");
                    Application.Quit();
                    return;
                default:
                    LoadScene(Scenes.Menu);
                    break;
            }
        }
        if(currentScene==Scenes.Menu)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                LoadScene(Scenes.Level1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LoadScene(Scenes.Level2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                LoadScene(Scenes.Level3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                LoadScene(Scenes.Level4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                LoadScene(Scenes.Level5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                LoadScene(Scenes.Level6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                LoadScene(Scenes.Level7);
            }
        }
    }

    [field: SerializeField]
    public Scenes currentScene { get; private set; } = Scenes.Menu;

    public void ReloadLevel()
    {
        switch (currentScene)
        {
            case Scenes.NONE:
                Debug.Log("currScene = NONE");
                break;
            case Scenes.SampleScene:
            case Scenes.Menu:
                break;
            case Scenes.Level1:
            case Scenes.Level2:
            case Scenes.Level3:
            case Scenes.Level4:
            case Scenes.Level5:
            case Scenes.Level6:
            case Scenes.Level7:
                LoadScene(currentScene);
                break;
            default:
                Debug.LogError("currScene isnt normal");
                break;
        }
    }

    public void LoadScene(Scenes sceneToLoad)
    {
        StartCoroutine(LoadSceneAnimation(sceneToLoad));
    }

    IEnumerator LoadSceneAnimation(Scenes sceneToLoad)
    {
        animator.Play("Fade_Start");
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneToLoad.ToString());
        currentScene = sceneToLoad;
        animator.Play("Fade_End");
    }
    #endregion

    #region GameCompletion
    [field: SerializeField]
    public int mostLevelCompleted { private set; get; } = 0;

    
    #endregion

}

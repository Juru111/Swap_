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
            Destroy(SM);
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
        SimpleMenu,
        SampleScene,
        Menu,
        Level1,
        Level2,
        Level3
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentScene)
            {
                case Scenes.SimpleMenu:
                    Debug.Log("Quitnig...");
                    Application.Quit();
                    return;
                default:
                    LoadScene(Scenes.SimpleMenu);
                    break;
            }
        }
    }

    [field: SerializeField]
    public Scenes currentScene { get; private set; } = Scenes.SimpleMenu;

    public void ReloadLevel()
    {
        switch (currentScene)
        {
            case Scenes.NONE:
                Debug.Log("currScene = NONE");
                break;
            case Scenes.SimpleMenu:
            case Scenes.SampleScene:
            case Scenes.Menu:
                break;
            case Scenes.Level1:
            case Scenes.Level2:
            case Scenes.Level3:
                LoadScene(currentScene);
                break;
            default:
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

}

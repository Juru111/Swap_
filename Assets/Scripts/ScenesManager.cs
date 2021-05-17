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

}

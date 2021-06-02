using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        DoSingletonLogic();
        GM.WindowsManager = FindObjectOfType<WindowsManager>();
        mostLevelCompleted = PlayerPrefs.GetInt("mostLevelCompleted", 0);
    }
    private void Update()
    {
        HandleEscKey();
        if (currentScene == ScenesTypes.Menu)
        {
            HandleNumericCheats();
            HandleNumericKeys();
        }
    }

    private void HandleEscKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentScene)
            {
                case ScenesTypes.Menu:
                    if(WindowsManager.CurrWindow == WindowTypes.NONE)
                    {
                        Debug.Log("Quitnig...");
                        Application.Quit();
                    }
                    else
                    {
                        WindowsManager.CloseWindow();
                    }
                    return;
                default:
                    LoadScene(ScenesTypes.Menu);
                    break;
            }

        }
    }

    private void HandleNumericCheats()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                LoadScene(ScenesTypes.Level1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LoadScene(ScenesTypes.Level2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                LoadScene(ScenesTypes.Level3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                LoadScene(ScenesTypes.Level4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                LoadScene(ScenesTypes.Level5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                LoadScene(ScenesTypes.Level6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                LoadScene(ScenesTypes.Level7);
            }
        }
    }

    private void HandleNumericKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && mostLevelCompleted >= 0)
        {
            LoadScene(ScenesTypes.Level1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && mostLevelCompleted >= 1)
        {
            LoadScene(ScenesTypes.Level2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && mostLevelCompleted >= 2)
        {
            LoadScene(ScenesTypes.Level3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && mostLevelCompleted >= 3)
        {
            LoadScene(ScenesTypes.Level4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && mostLevelCompleted >= 4)
        {
            LoadScene(ScenesTypes.Level5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && mostLevelCompleted >= 5)
        {
            LoadScene(ScenesTypes.Level6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && mostLevelCompleted >= 6)
        {
            LoadScene(ScenesTypes.Level7);
        }
    }

    #region Singleton
    public static GameManager GM;
    private void DoSingletonLogic()
    {
        if (GM != null)
        {
            //GM = this;
            Destroy(gameObject); // pocz¹tkowa tylko ta linijka
            return;
        }
        else
        {
            GM = this;
        }
        DontDestroyOnLoad(this);
    }
    #endregion

    #region ScenesManagment
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private TMP_Text loadLevelText;

    public enum ScenesTypes
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

    [field: SerializeField]
    public ScenesTypes currentScene { get; private set; } = ScenesTypes.Menu;

    public void ReloadLevel()
    {
        switch (currentScene)
        {
            case ScenesTypes.NONE:
                Debug.Log("currScene = NONE");
                break;
            case ScenesTypes.SampleScene:
            case ScenesTypes.Menu:
                break;
            case ScenesTypes.Level1:
            case ScenesTypes.Level2:
            case ScenesTypes.Level3:
            case ScenesTypes.Level4:
            case ScenesTypes.Level5:
            case ScenesTypes.Level6:
            case ScenesTypes.Level7:
                LoadScene(currentScene);
                break;
            default:
                Debug.LogError("currScene isnt normal");
                break;
        }
    }

    public void LoadScene(ScenesTypes sceneToLoad, float waitInFade = 0.2f, string loadString = "Swap")
    {
        if(waitInFade == 0.2f)
        {
            GM.SoundManager.PlaySound(SoundTypes.LoadScene);
        }
        StartCoroutine(LoadSceneAnimation(sceneToLoad, waitInFade, loadString));
    }

    IEnumerator LoadSceneAnimation(ScenesTypes sceneToLoad, float waitInFade, string loadString)
    {
        loadLevelText.text = loadString;
        animator.Play("Fade_Start");
        yield return new WaitForSeconds(waitInFade);
        SceneManager.LoadScene(sceneToLoad.ToString());
        currentScene = sceneToLoad;
        animator.Play("Fade_End");
    }
    #endregion

    #region GameCompletion
    [field: SerializeField]
    public int mostLevelCompleted { private set; get; } = 0;

    public void ResetGameCompletion()
    {
        PlayerPrefs.SetInt("mostLevelCompleted", 0);
        LoadScene(ScenesTypes.Menu);
    }

    public void SetLevelComletion(int levelCompleted)
    {
        if(levelCompleted > mostLevelCompleted)
        {
            mostLevelCompleted = levelCompleted;
        }
    }
    #endregion

    #region Music
    [SerializeField]
    private AudioSource musicSource;

    public void AdjustMusicVolume(float _volume)
    {
        musicSource.volume = _volume/2;
    }
    #endregion

    #region Sound
    public SoundManager SoundManager;
    #endregion

    #region WindowManager
    [SerializeField]
    public WindowsManager WindowsManager;
    #endregion
}

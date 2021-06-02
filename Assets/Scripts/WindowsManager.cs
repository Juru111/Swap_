using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject creditsPanel;
    private GameObject currWindowObject;
    public WindowTypes CurrWindow { private set; get; }

    private SoundManager SoundManager;

    private void Awake()
    {
        SoundManager = GameManager.GM.SoundManager;
    }

    public void OpenWindow(WindowTypes windowType)
    {
        if(windowType == WindowTypes.NONE)
        {
            Debug.LogError("Z³y typ okna");
            return;
        }
        CurrWindow = windowType;
        
        switch (windowType)
        {
            case WindowTypes.Settings:
                settingsPanel.SetActive(true);
                currWindowObject = settingsPanel;
                SoundManager.RefreshAudioSliders();
                break;
            case WindowTypes.Controls:
                controlsPanel.SetActive(true);
                currWindowObject = controlsPanel;
                break;
            case WindowTypes.Credits:
                creditsPanel.SetActive(true);
                currWindowObject = creditsPanel;
                break;
            default:
                break;
        }
    }

    public void CloseWindow()
    {
        if (CurrWindow == WindowTypes.Settings)
        {
            PlayerPrefs.SetFloat("MusicVolume", SoundManager.GiveMusicVolume());
            PlayerPrefs.SetFloat("SfxVolume", SoundManager.GiveSfxVolume());
        }
        currWindowObject.SetActive(false);
        CurrWindow = WindowTypes.NONE;
    }
}

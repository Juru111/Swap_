using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void ApplicationQuitCall()
    {
        GameManager.GM.SoundManager.PlaySound(SoundTypes.ButtonClick);
        Debug.Log("Quitnig...");
        Application.Quit();
    }
}

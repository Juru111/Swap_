using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void ApplicationQuit()
    {
        Debug.Log("Quitnig...");
        Application.Quit();
    }
}

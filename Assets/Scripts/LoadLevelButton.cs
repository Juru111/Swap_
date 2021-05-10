using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelButton : MonoBehaviour
{
    public ScenesManager.Scenes sceneToLoad;
    public void LoadlevelCall()
    {
        ScenesManager.SM.LoadScene(sceneToLoad);
    }
}

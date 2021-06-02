using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinIndicator : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup myCanvasGroup;
    private void Awake()
    {
        if( PlayerPrefs.GetInt("mostLevelCompleted", 0) >= 7)
        {
            myCanvasGroup.alpha = 1;
        }
    }
}

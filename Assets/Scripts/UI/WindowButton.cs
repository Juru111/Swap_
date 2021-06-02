using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowButton : MonoBehaviour
{
    [SerializeField]
    private WindowTypes windowTypeToOpen;

    public void OpenWindowCall()
    {
        GameManager.GM.SoundManager.PlaySound(SoundTypes.ButtonClick);
        GameManager.GM.WindowsManager.OpenWindow(windowTypeToOpen);
    }
}

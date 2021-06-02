using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void CloseWindowCall()
    {
        GameManager.GM.SoundManager.PlaySound(SoundTypes.ButtonClick);
        GameManager.GM.WindowsManager.CloseWindow();
    }
}

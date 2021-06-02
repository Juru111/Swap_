using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetProgresButton : MonoBehaviour
{
    public void ResetCall()
    {
        GameManager.GM.SoundManager.PlaySound(SoundTypes.ButtonClick);
        GameManager.GM.ResetGameCompletion();
    }
}

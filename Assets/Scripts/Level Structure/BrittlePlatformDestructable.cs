using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittlePlatformDestructable : MonoBehaviour, IDestroyable
{
    [SerializeField]
    private BrittlePlatform mainBrittlePlatform;

    public void AttackMe()
    {
        if (!mainBrittlePlatform.isInCorr)
        {
            mainBrittlePlatform.BrittleMe(0, mainBrittlePlatform.unBrittleTime);
        }
    }
}

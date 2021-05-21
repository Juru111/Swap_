using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittlePlatformDestructable : MonoBehaviour, IDestroyable
{
    [SerializeField]
    private BrittlePlatform mainBrittlePlatform;

    public void AttackMe()
    {
        if (!mainBrittlePlatform.isBrittling)
        {
            Debug.Log("Brettle by attack");
            StartCoroutine(mainBrittlePlatform.BrittleMe(0, mainBrittlePlatform.unBrittleTime));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour, IDestroyable
{
    public void AttackMe()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour, IDestroyable
{
    [SerializeField]
    private GameObject barrierParticle;

    public void AttackMe()
    {
        barrierParticle.SetActive(true);
        gameObject.SetActive(false);
    }
}

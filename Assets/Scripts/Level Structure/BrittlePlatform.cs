using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittlePlatform : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer platformSpriteRenderer;
    [SerializeField]
    private Collider2D platformCollider;
    [SerializeField]
    private GameObject brittleParticle;
    [SerializeField]
    private float brittleTime = 1f;
    [field: SerializeField]
    public float unBrittleTime { private set; get; } = 3f;
    public bool isBrittling { private set; get; } = false;

    private int playerInCount = 0;

    private void OnTriggerEnter2D(Collider2D collisionColider)
    {
        if (collisionColider.TryGetComponent(out Player player))
        {
            playerInCount++;
            if(!isBrittling)
            {
                StartCoroutine(BrittleMe(brittleTime, unBrittleTime));
            } 
        }
    }
    private void OnTriggerExit2D(Collider2D collisionColider)
    {
        if (collisionColider.TryGetComponent(out Player player))
        {
            playerInCount--;
        }
    }


    public IEnumerator BrittleMe(float _brittleTime, float _unBrittleTime)
    {
        isBrittling = true;

        brittleParticle.SetActive(true);
        yield return new WaitForSeconds(_brittleTime/2);
        brittleParticle.SetActive(true);
        yield return new WaitForSeconds(_brittleTime/2);
        brittleParticle.SetActive(true);
        platformCollider.enabled = false;
        platformSpriteRenderer.enabled = false;
        while(playerInCount > 0)
        {
            yield return null;
        }
        StartCoroutine(UnBrittleMe(_unBrittleTime));

        isBrittling = false;
    }

    public IEnumerator UnBrittleMe(float _unBrittleTime)
    {
        yield return new WaitForSeconds(_unBrittleTime);
        platformCollider.enabled = true;
        platformSpriteRenderer.enabled = true;
    }
}

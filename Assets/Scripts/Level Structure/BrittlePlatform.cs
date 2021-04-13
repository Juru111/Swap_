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
    private float brittleTime = 1f;
    [field: SerializeField]
    public float unBrittleTime { private set; get; } = 3f;
    public bool isInCorr { private set; get; } = false;

    private void OnTriggerEnter2D(Collider2D collisionColider)
    {
        if (collisionColider.TryGetComponent(out Player player) && !isInCorr)
        {
            StartCoroutine(BrittleMe(brittleTime, unBrittleTime));
        }
    }

    public IEnumerator BrittleMe(float _brittleTime, float _unBrittleTime)
    {
        isInCorr = true;

        yield return new WaitForSeconds(_brittleTime); //podzieliæ na mniejsze kawa³ki - ró¿ne sprite-y
        platformCollider.enabled = false;
        platformSpriteRenderer.enabled = false;
        yield return new WaitForSeconds(_unBrittleTime);
        platformCollider.enabled = true;
        platformSpriteRenderer.enabled = true;

        isInCorr = false;
    }
}

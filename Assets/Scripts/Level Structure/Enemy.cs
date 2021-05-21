using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDestroyable
{
    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;
    [SerializeField]
    private GameObject markerPrefab;
    [SerializeField]
    private GameObject enemyParticlePrefab;
    private Vector3 target;
    [SerializeField]
    private float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        target = pointB.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        if(Vector3.Distance(transform.position, target) < 0.05)
        {
            SwitchTarget();
        }
    }

    private void OnTriggerEnter2D(Collider2D collisionCollider)
    {
        if (collisionCollider.TryGetComponent(out Player player))
        {
            player.KillMe();
        }
    }

    private void SwitchTarget()
    {
        if (target == pointA.position)
        {
            target = pointB.position;
        }
        else
        {
            target = pointA.position;
        }
    }

    public void AttackMe()
    {
        Instantiate(markerPrefab, transform.position, Quaternion.identity);
        Instantiate(enemyParticlePrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}

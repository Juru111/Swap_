using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapManager : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private Rigidbody2D rb2Dplayer1;
    private Rigidbody2D rb2Dplayer2;
    Vector3 previous1Positon;
    Vector2 previosu1Velocity;

    [SerializeField] private GameObject swapParticlePrefab;

    private bool swap = false;

    private void Awake()
    {
        rb2Dplayer1 = player1.GetComponent<Rigidbody2D>();
        rb2Dplayer2 = player2.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            swap = true;
        }
    }

    private void FixedUpdate()
    {
        if(swap)
        {
            GameManager.GM.SoundManager.PlaySound(SoundTypes.Swap);
            previous1Positon = player1.transform.position;
            previosu1Velocity = rb2Dplayer1.velocity;

            Instantiate(swapParticlePrefab, player1.transform.position, Quaternion.identity);
            Instantiate(swapParticlePrefab, player2.transform.position, Quaternion.identity);

            player1.transform.position = player2.transform.position;
            player2.transform.position = previous1Positon;

            rb2Dplayer1.velocity = rb2Dplayer2.velocity;
            rb2Dplayer2.velocity = previosu1Velocity;

            swap = false;
        }
    }
}

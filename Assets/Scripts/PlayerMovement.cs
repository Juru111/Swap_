using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputConfig myInputConfig;

    [SerializeField]
    private CharacterController2D controller;
    [SerializeField]
    private float runSpeed = 40f;

    [SerializeField]
    private float h_Movement = 0f;
    [SerializeField]
    private bool jump = false;
    [SerializeField]
    private float jumpCooldown = 0.5f;
    private float jumpCooldownLeft = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(myInputConfig.JumpKey) && jumpCooldownLeft < 0)
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        CalculateHorizontalMovment();
        jumpCooldownLeft -= Time.fixedDeltaTime;


        controller.Move(h_Movement * Time.fixedDeltaTime, false, jump);
        if(jump)
        {
            jump = false;
            jumpCooldownLeft = jumpCooldown;
        }
        


    }

    private void CalculateHorizontalMovment()
    {
        h_Movement = 0f;
        if (Input.GetKey(myInputConfig.LeftKey))
        {
            h_Movement--;
        }
        if (Input.GetKey(myInputConfig.RightKey))
        {
            h_Movement++;
        }
        h_Movement *= runSpeed;
    }
}

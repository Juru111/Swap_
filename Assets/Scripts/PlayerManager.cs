using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private InputConfig myInputConfig;

    [SerializeField]
    private CharacterController2D controller;
    [SerializeField]
    private Animator attackCircleAnimator;
    [SerializeField]
    private float runSpeed = 40f;
    [SerializeField]
    private float jumpCooldown = 0.5f;

    private float h_Movement = 0f;
    private bool jump = false;
    private float jumpCooldownLeft = 0f;
    private bool isAttacking = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(myInputConfig.JumpKey) && jumpCooldownLeft < 0)
        {
            jump = true;
        }
        if (Input.GetKey(myInputConfig.AttackKey))
        {
            isAttacking = true;
        }

        attackCircleAnimator.SetBool("isAttacking", isAttacking);
        if (attackCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("AttackCircle_StayBig"))
        {
            Debug.Log("ready to fire" + gameObject.name);
            //ready to fire
        }
    }

    void FixedUpdate()
    {
        CalculateHorizontalMovment();
        jumpCooldownLeft -= Time.fixedDeltaTime;

        controller.Move(h_Movement * Time.fixedDeltaTime, isAttacking, jump);
        if(jump)
        {
            jump = false;
            jumpCooldownLeft = jumpCooldown;
        }
        if(isAttacking)
        {
            isAttacking = false;
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

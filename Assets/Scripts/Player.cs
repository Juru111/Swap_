using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

    public ItemTypes itemHeld { private set; get; } = ItemTypes.NONE;
    public ItemColors itemHeldColor { private set; get; } = ItemColors.NONE;

    private float h_Movement = 0f;
    private bool jump = false;
    private bool grab = false;
    private float jumpCooldownLeft = 0f;
    private bool isAttacking = false;

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
        if (Input.GetKeyDown(myInputConfig.GrabKey))
        {
            grab = true;
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

        controller.Move(h_Movement * Time.fixedDeltaTime, isAttacking, jump, grab);
        if(jump)
        {
            jump = false;
            jumpCooldownLeft = jumpCooldown;
        }
        isAttacking = false;
        grab = false;
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

    public void SetMyItem(ItemTypes itemType, ItemColors itemColor)
    {
        itemHeld = itemType;
        itemHeldColor = itemColor;
        //tu mo¿na dodaæ te¿ w³¹cznie wizualnego indyfikatora trzymanego przedmiotu
    }
}

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
    public GameObject itemHeldIndicator;
    [SerializeField]
    private SpriteRenderer itemHeldSpriteRenderer;
    [SerializeField]
    private Collider2D attackCollider;
    private GameManager gameManager;
    [SerializeField]
    private ContactFilter2D destructableObjects = new ContactFilter2D();

    [SerializeField]
    private float runSpeed = 40f;
    [SerializeField]
    private float jumpCooldown = 0.5f;

    [field: SerializeField]
    public ItemTypes itemHeld { private set; get; } = ItemTypes.NONE;
    [field: SerializeField]
    public ItemColors itemHeldColor { private set; get; } = ItemColors.NONE;

    private float h_Movement = 0f;
    private bool jump = false;
    private bool grab = false;
    private float jumpCooldownLeft = 0f;
    private bool isAttacking = false;
    [SerializeField]
    private bool isAlive = true;
    [SerializeField]
    private bool inToxic = false;
    [SerializeField]
    private float toxicationLevel;
    [SerializeField]
    private float maxToxicationLevel;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

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
            if (Input.GetKeyUp(myInputConfig.AttackKey))
            {
                Collider2D[] attackedObjects = new Collider2D[8];
                _ = attackCollider.OverlapCollider(destructableObjects, attackedObjects);
                foreach (Collider2D collider in attackedObjects)
                {
                    if (collider != null && collider.TryGetComponent(out IDestroyable destroyable))
                    {
                        destroyable.AttackMe();
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(inToxic)
        {
            toxicationLevel += Time.deltaTime;
            if(toxicationLevel > maxToxicationLevel)
            {
                KillMe();
            }
        }
        else
        {
            if(toxicationLevel > 0f)
            {
                toxicationLevel -= Time.deltaTime*2;
            }
            else
            {
                toxicationLevel = 0f;
            }
        }


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

    public void SetMyItem(ItemTypes itemType, ItemColors itemColor, Sprite itemSprite)
    {
        if (itemType != ItemTypes.Marker)
        {
            itemHeld = itemType;
            itemHeldColor = itemColor;
            //tu mo¿na dodaæ te¿ w³¹cznie wizualnego indyfikatora trzymanego przedmiotu
            if (itemType != ItemTypes.NONE)
            {
                itemHeldIndicator.SetActive(true);
                itemHeldSpriteRenderer.sprite = itemSprite;
            }
            else
            {
                itemHeldIndicator.SetActive(false);
            }
        }
        else
        {
            gameManager.AddPoints(1);
        }
    }

    public void KillMe()
    {
        
        if(isAlive)
        {
            //puff particle
            //zabicie (uktycie) playera
            gameManager.LevelFailed();
            isAlive = false;
        }
        
    }

    public void SetInToxic(bool _inToxic)
    {
        inToxic = _inToxic;
    }

}

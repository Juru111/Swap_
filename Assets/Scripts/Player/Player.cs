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
    private Animator spriteAnimator;
    [SerializeField]
    public GameObject itemHeldIndicator;
    [SerializeField]
    private SpriteRenderer itemHeldSpriteRenderer;
    [SerializeField]
    private Collider2D attackCollider;
    [SerializeField]
    private LevelEventHandler levelEventHandler;
    [SerializeField]
    private ContactFilter2D destructableObjects = new ContactFilter2D();

    [SerializeField]
    private GameObject deathParticlePrefab;
    [SerializeField]
    private GameObject attackParticlePrefab;

    [SerializeField]
    private float runSpeed = 40f;
    [SerializeField]
    private float jumpCooldown = 0.5f;

    [field: SerializeField]
    public ItemTypes itemHeld { private set; get; } = ItemTypes.NONE;
    [field: SerializeField]
    public ItemColors itemHeldColor { private set; get; } = ItemColors.NONE;
    [field: SerializeField]
    public bool isHoldingItem { private set; get; } = false;

    private float h_Movement = 0f;
    private bool jump = false;
    private bool grab = false;
    private float jumpCooldownLeft = 0f;
    private bool isAttacking = false;
    private bool attackReady = false;
    [SerializeField]
    private bool isAlive = true;
    [SerializeField]
    private bool inToxic = false;
    [SerializeField]
    private float toxicationLevel;
    [SerializeField]
    private float maxToxicationLevel;

    private void Update()
    {
        if (Input.GetKey(myInputConfig.JumpKey) && jumpCooldownLeft < 0)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }

        if (Input.GetKey(myInputConfig.AttackKey))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (Input.GetKeyDown(myInputConfig.GrabKey))
        {
            grab = true;
        }
        

        attackCircleAnimator.SetBool("isAttacking", isAttacking);
        attackCircleAnimator.SetBool("isHoldingItem", isHoldingItem);
        if (attackCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("AttackCircle_StayBig"))
        {
            attackReady = true;

            //moment of dealing damage
            if (Input.GetKeyUp(myInputConfig.AttackKey))
            {
                Instantiate(attackParticlePrefab, transform.position, Quaternion.identity);
                Collider2D[] attackedObjects = new Collider2D[8];
                _ = attackCollider.OverlapCollider(destructableObjects, attackedObjects);
                foreach (Collider2D collider in attackedObjects)
                {
                    if (collider != null && collider.TryGetComponent(out IDestroyable destroyable))
                    {
                        destroyable.AttackMe();
                    }
                }
                attackReady = false;
            }
        }
        else
        {
            
        }
        spriteAnimator.SetFloat("Speed", Mathf.Abs(h_Movement));
        spriteAnimator.SetBool("IsInAir", !controller.m_Grounded);
        spriteAnimator.SetBool("IsGoingUp", controller.m_IsGoingUp);
        spriteAnimator.SetBool("IsAttacking", isAttacking);
        spriteAnimator.SetBool("IsGrabing", controller.m_IsGrabing);
        spriteAnimator.SetBool("StartGrab", grab);
        spriteAnimator.SetBool("IsHoldingItem", isHoldingItem);
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

        controller.Move(h_Movement * Time.fixedDeltaTime, isAttacking, attackReady , jump, grab);
        if(jump)
        {
            jump = false;
            jumpCooldownLeft = jumpCooldown;
        }


        

        //isAttacking = false;
        grab = false;

        
    }


    private void CalculateHorizontalMovment()
    {
        h_Movement = 0f;
        if (!isAttacking)
        {
            if (Input.GetKey(myInputConfig.LeftKey))
            {
                h_Movement--;
            }
            if (Input.GetKey(myInputConfig.RightKey))
            {
                h_Movement++;
            }
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
                isHoldingItem = true; //characterControler ustawia t¹ wartoœæ na true nawet wczeœniej - w momencie zaatakowania (na potrzeby animatora)
            }
            else
            {
                itemHeldIndicator.SetActive(false);
                isHoldingItem = false;
            }
        }
        else
        {
            levelEventHandler.AddPoints(1);
        }
    }

    public void KillMe()
    {
        
        if(isAlive)
        {
            //puff particle
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
            //zabicie (uktycie) playera
            
            //gameManager.LevelFailed();
            //LevelEventHandler.LevelFailed();
            if (ScenesManager.SM != null)
            {
                ScenesManager.SM.ReloadLevel();
            }
            isAlive = false;
            Debug.Log("Player Dead");
            gameObject.SetActive(false);
        }
        
    }

    public void SetInToxic(bool _inToxic)
    {
        inToxic = _inToxic;
    }

    public void SetHoldingItem(bool _isHoldingItem)
    {
        isHoldingItem = _isHoldingItem;
    }
}

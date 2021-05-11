using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected Rigidbody2D myRigidbody2D;
    public Sprite mySprite { protected set; get; }

    [SerializeField]
    protected ItemTypes myItemType = ItemTypes.NONE;
    [SerializeField]
    protected ItemColors myItemColor = ItemColors.NONE;
    [SerializeField]
    protected float throwForce = 300f;
    //[SerializeField]
    //protected float throwDistance = 1f;


    public ItemTypes MyItemType => myItemType;
    public ItemColors MyItemColor => myItemColor;

    public bool isTaken { protected set; get; } = false;

    protected void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>().sprite;
    }

    public virtual void GoToPlayer(Vector2 goToPosition, float time)
    {
        Sequence goToSequence = DOTween.Sequence();
        goToSequence.Append(transform.DOMove(goToPosition, time));
        goToSequence.OnComplete(DestroyMe);
        goToSequence.Play();
    }

    protected virtual void DestroyMe()
    {
        Destroy(gameObject);
    }

    public virtual void SetMyTakenStatus(bool _isTaken)
    {
        isTaken = _isTaken;
    }

    public virtual void TrowMe(bool isDirIsRight, float throwTime)
    {
        int factor;
        if(isDirIsRight)
        {
            factor = 1;
        }
        else
        {
            factor = -1;
        }
        myRigidbody2D.AddForce(new Vector2(throwForce * factor, 0));
        //transform.DOMove(transform.position + Vector3.right * throwDistance * factor, throwTime);
    }
}

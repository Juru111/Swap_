using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private ItemColors myColor = ItemColors.NONE;
    [SerializeField]
    private float moveDoorTime;
    [SerializeField]
    private int rotateFactor;
    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;

    private bool isOnPointA = true;
    private bool inMove = false;

    private void OnTriggerEnter2D(Collider2D collisionColider)
    {
        if (!inMove && collisionColider.TryGetComponent(out Player player))
        {
            if (player.itemHeld.Equals(ItemTypes.Key) && player.itemHeldColor.Equals(myColor))
            {
                GameManager.GM.SoundManager.PlaySound(SoundTypes.DoorStart);
                MoveDoor(moveDoorTime);
            }
        }
    }

    private void MoveDoor(float time)
    {
        inMove = true;
        Sequence moveDoor = DOTween.Sequence();
        moveDoor.SetUpdate(UpdateType.Fixed);
        if (isOnPointA)
        {
            moveDoor = DOTween.Sequence();
            moveDoor.SetUpdate(UpdateType.Fixed);
            moveDoor.Append(transform.DOMove(pointB.position, time));
            moveDoor.Insert(0, transform.DORotate(new Vector3(0, 0, 90 * rotateFactor), time, RotateMode.LocalAxisAdd));
            moveDoor.OnComplete(MoveComplete);
            moveDoor.Play();
        }
        else
        {
            moveDoor = DOTween.Sequence();
            moveDoor.SetUpdate(UpdateType.Fixed);
            moveDoor.Append(transform.DOMove(pointA.position, time));
            moveDoor.Insert(0, transform.DORotate(new Vector3(0, 0, 90 * -rotateFactor), time, RotateMode.LocalAxisAdd));
            moveDoor.OnComplete(MoveComplete);
            moveDoor.Play();
        }
    }

    private void MoveComplete()
    {
        isOnPointA = !isOnPointA;
        inMove = false;
    }
}

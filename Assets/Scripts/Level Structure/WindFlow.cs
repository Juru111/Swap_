using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    NONE = 0,
    Up = 1,
    Right = 2,
}

public class WindFlow : MonoBehaviour
{
    [SerializeField]
    private Direction myDirection = Direction.NONE;
    [SerializeField]
    private float windSpeed = 1f;

    private void OnTriggerStay2D(Collider2D collisionColider)
    {
        if (collisionColider.TryGetComponent(out CharacterController2D playerControler)
            && collisionColider.TryGetComponent(out Player player)
            && !player.itemHeld.Equals(ItemTypes.Armor))
        {
            playerControler.WindMovement(myDirection, windSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D playerControler)
        && collision.TryGetComponent(out Player player))
        {
            playerControler.WindMovement(myDirection, 0);
        }
    }
}

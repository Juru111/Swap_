using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    NONE = 0,
    Up = 1,
    Left = 2,
    Right = 3,
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
            && collisionColider.TryGetComponent(out Player player))
        {
            if (!player.itemHeld.Equals(ItemTypes.Armor))
            {
                playerControler.WindMovement(myDirection, windSpeed);
            }
        }
    }
}

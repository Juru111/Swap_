using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{   
    [SerializeField]
    private ItemColors myColor = ItemColors.NONE;

    private void OnTriggerStay2D(Collider2D collisionColider)
    {
        if (collisionColider.TryGetComponent(out Player player))
        {
            if (!(player.itemHeld.Equals(ItemTypes.Crystal) && player.itemHeldColor.Equals(myColor)))
            {
                player.SetInToxic(true);
            }
            else
            {
                player.SetInToxic(false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collisionColider)
    {
        if (collisionColider.TryGetComponent(out Player player))
        {
            player.SetInToxic(false);
        }
    }
}

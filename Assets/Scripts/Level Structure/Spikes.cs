using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collisionColider)
    {
        if (collisionColider.TryGetComponent(out Player player))
        {
            if (!player.itemHeld.Equals(ItemTypes.Armor))
            {
                player.KillMe();
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : Item
{
    public override void GoToPlayer(Vector2 goToPosition, float time)
    {
        GameManager.GM.SoundManager.PlaySound(SoundTypes.MarkerPickUp);
        base.GoToPlayer(goToPosition, time);
    }
}

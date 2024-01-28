using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableHolder : DraggableHolder<string>
{
    protected override Draggable<string> getSelectedDraggable()
    {
        Debug.Log(Player.getSelectedConsumable());
        return Player.getSelectedConsumable();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableHolder : DraggableHolder
{
    protected override Draggable getSelectedDraggable()
    {
        return Player.getSelectedConsumable();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Consumable : Draggable<string>
{
    [SerializeField] private Image icon;
    [SerializeField] private int cost;
    [SerializeField] private string text;
    private UnityAction action;

    public Consumable setSlot(Transform slot)
    {
        this.slot = slot;
        return this;
    }

    public void setup(Sprite sprite, UnityAction action)
    {
        this.action = action;
        icon.sprite = sprite;
        if (Player.getGold() < cost) canBeDragged = false;
    }

    public void Consume()
    {
        action.Invoke();
    }

    protected override void onBeginDrag()
    {
        Player.setSelectedConsumable(this);
    }

    protected override void onEndDrag()
    {
        Player.setSelectedConsumable(null);
    }
}

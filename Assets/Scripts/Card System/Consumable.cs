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
    private bool bought = false;

    public Consumable setSlot(Transform slot)
    {
        this.slot = slot;
        return this;
    }

    public Consumable setup(Sprite sprite, UnityAction action)
    {
        this.action = action;
        icon.sprite = sprite;
        if (Player.getGold() < cost) canBeDragged = false;
        return this;
    }

    public void setCost(int cost)
    {
        this.cost = cost;
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

    protected override void onPlaced()
    {
        bought = true;
    }

    protected override bool onDrop()
    {
        if (!bought) return true;
        Consume();
        Destroy(gameObject);
        return false;
    }
}

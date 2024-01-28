using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Consumable : Draggable<string>
{
    [SerializeField] private Image icon;
    private int cost;
    [SerializeField] private TextMeshProUGUI costDisplay;
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
        return this;
    }

    public void setCost(int cost)
    {
        this.cost = cost;
        if (Player.getGold() < cost) canBeDragged = false;
        costDisplay.SetText(cost.ToString());
    }

    public void updateCanBeDragged()
    {
        if (Player.getGold() < cost) canBeDragged = false;
    }

    public void Consume()
    {
        action.Invoke();
    }

    protected override void onBeginDrag()
    {
        if (!bought) costDisplay.gameObject.SetActive(false);
        Player.setSelectedConsumable(this);
    }

    protected override void onEndDrag()
    {
        if (!bought) costDisplay.gameObject.SetActive(true);
        Player.setSelectedConsumable(null);
    }

    protected override void onPlaced()
    {
        Player.SpendGold(cost);
        bought = true;
        Merchant.instance.Buy(this);
        Destroy(costDisplay.gameObject);
    }

    protected override bool onDrop()
    {
        if (!bought) return true;
        Consume();
        Destroy(gameObject);
        return false;
    }
}

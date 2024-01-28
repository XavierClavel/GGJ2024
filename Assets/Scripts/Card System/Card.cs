using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : Draggable<CardHandler>
{
    
    [SerializeField] private TextMeshProUGUI titleDisplay;
    [SerializeField] private Image icon;
    [SerializeField] private Image bandeau;
    private Vector2 startPos;
    private CardHandler cardHandler;
    private float hiddenPos = -300f;
    private float inHolderScale = 0.7f;
    

    public Card setup(string key, Transform slot)
    {
        cardHandler = DataManager.dictKeyToCard[key];
        titleDisplay.SetText(cardHandler.getKey());
        this.slot = slot;
        startPos = rectTransform.anchoredPosition;
        icon.sprite = cardHandler.getIcon();
        icon.color = cardHandler.getAccentColor();
        bandeau.color = cardHandler.getAccentColor();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, hiddenPos);

        gameObject.name = key;

        return this;
    }

    public void Show()
    {
        rectTransform.DOAnchorPosY(0f, 1f);
    }

    public void Hide()
    {
        rectTransform.DOAnchorPosY(hiddenPos, 1f);
    }


    public CardHandler getCardInfo()
    {
        return cardHandler;
    }

    
    protected override void onBeginDrag()
    {
        Player.setSelectedCard(this);
    }

    protected override void onEndDrag()
    {
        Debug.Log("Pointer up");
        if (hoverDraggableHolder == null)
        {
            AttachToSlot();
        } else if (!hoverDraggableHolder.isFree(this))
        {
            AttachToSlot();
            hoverDraggableHolder.hoverDraggable = null;
            hoverDraggableHolder = null;
        }
        else
        {
            if (selectedDraggableHolder != null && selectedDraggableHolder != hoverDraggableHolder)
            {
                selectedDraggableHolder.selectedDraggable = null;
                selectedDraggableHolder = null;
            }
            AttachToDraggableHolder(hoverDraggableHolder);
            selectedDraggableHolder = hoverDraggableHolder;
            selectedDraggableHolder.selectedDraggable = this;
            hoverDraggableHolder.hoverDraggable = null;
            hoverDraggableHolder = null;
        }
        Player.setSelectedCard(null);
    }

    private void AttachToSlot()
    {
        if (selectedDraggableHolder != null)
        {
            selectedDraggableHolder.selectedDraggable = null;
            selectedDraggableHolder = null;
        }
        rectTransform.SetParent(slot);
        rectTransform.anchorMin = 0.5f * Vector2.one;
        rectTransform.anchorMax = 0.5f * Vector2.one;
        rectTransform.anchoredPosition = startPos;
    }

    

    private void OnDestroy()
    {
        if(slot != null) Destroy(slot.gameObject);
    }
}
 
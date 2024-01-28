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
        Player.setSelectedCard(null);
    }
    

    private void OnDestroy()
    {
        if(slot != null) Destroy(slot.gameObject);
    }
}
 
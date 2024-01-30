using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Card : Draggable<CardHandler>, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField] private TextMeshProUGUI titleDisplay;
    [SerializeField] private Image icon;
    [SerializeField] private Image bandeau;
    private float startPos;
    private float highlightedPos;
    private CardHandler cardHandler;
    private float hiddenPos = -300f;
    private float inHolderScale = 0.7f;
    private bool dragged = false;
    private Tween tween;
    

    public Card setup(string key, Transform slot)
    {
        cardHandler = DataManager.dictKeyToCard[key];
        titleDisplay.SetText(cardHandler.getKey());
        this.slot = slot;
        startPos = Random.Range(-20f, 20f);
        icon.sprite = cardHandler.getIcon();
        icon.color = cardHandler.getAccentColor();
        bandeau.color = cardHandler.getAccentColor();
        image.color = cardHandler.getBackgroundColor();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, hiddenPos);

        gameObject.name = key;

        return this;
    }

    public void Show()
    {
        rectTransform.DOAnchorPosY(startPos, 1f);
    }

    public void Hide()
    {
        image.raycastTarget = false;
        rectTransform.DOAnchorPosY(hiddenPos, 1f);
    }


    public CardHandler getCardInfo()
    {
        return cardHandler;
    }

    
    protected override void onBeginDrag()
    {
        dragged = true;
        tween?.Kill();
        Player.setSelectedCard(this);
    }

    protected override void onEndDrag()
    {
        dragged = false;
        Player.setSelectedCard(null);
        AudioManager.PlaySfx("PoseCard");
    }
    

    private void OnDestroy()
    {
        if(slot != null) Destroy(slot.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (dragged || selectedDraggableHolder != null) return;
        tween = rectTransform.DOAnchorPosY(200f, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (dragged || selectedDraggableHolder != null) return;
        tween = rectTransform.DOAnchorPosY(startPos, 0.5f);
    }
}
 
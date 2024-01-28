using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector] public CardHolder hoverCardHolder = null;
    [HideInInspector] public CardHolder selectedCardHolder = null;
    public RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI titleDisplay;
    [SerializeField] private Image image;
    [SerializeField] private Image icon;
    [SerializeField] private Image bandeau;
    private Transform slot;
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

    
    public void OnBeginDrag(PointerEventData data)
    {
        image.raycastTarget = false;
        Debug.Log("Pointer down");
        Player.setSelectedCard(this);
        transform.SetParent(EnnemyManager.instance.canvas);
    }

    public void OnDrag(PointerEventData data)
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        image.raycastTarget = true;
        Debug.Log("Pointer up");
        if (hoverCardHolder == null)
        {
            AttachToSlot();
        } else if (!hoverCardHolder.isFree(this))
        {
            AttachToSlot();
            hoverCardHolder.hoverCard = null;
            hoverCardHolder = null;
        }
        else
        {
            if (selectedCardHolder != null && selectedCardHolder != hoverCardHolder)
            {
                selectedCardHolder.selectedCard = null;
                selectedCardHolder = null;
            }
            AttachToCardHolder(hoverCardHolder);
            selectedCardHolder = hoverCardHolder;
            selectedCardHolder.selectedCard = this;
            hoverCardHolder.hoverCard = null;
            hoverCardHolder = null;
        }
        Player.setSelectedCard(null);
    }

    private void AttachToSlot()
    {
        if (selectedCardHolder != null)
        {
            selectedCardHolder.selectedCard = null;
            selectedCardHolder = null;
        }
        rectTransform.SetParent(slot);
        rectTransform.anchorMin = 0.5f * Vector2.one;
        rectTransform.anchorMax = 0.5f * Vector2.one;
        rectTransform.anchoredPosition = startPos;
    }

    private void AttachToCardHolder(CardHolder cardHolder)
    {
        rectTransform.SetParent(cardHolder.rectTransform);
        rectTransform.anchorMin = 0.5f * Vector2.one;
        rectTransform.anchorMax = 0.5f * Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
    }

    private void OnDestroy()
    {
        if(slot != null) Destroy(slot.gameObject);
    }
}
 
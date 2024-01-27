using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI titleDisplay;
    [SerializeField] private Image image;
    [SerializeField] private Image icon;
    [SerializeField] private Image bandeau;
    private Transform slot;
    private Vector2 startPos;
    private CardHandler cardHandler;
    private int siblingIndex;

    public Card setup(string key, Transform slot)
    {
        cardHandler = DataManager.dictKeyToCard[key];
        titleDisplay.SetText(cardHandler.getKey());
        this.slot = slot;
        startPos = rectTransform.anchoredPosition;
        icon.sprite = cardHandler.getIcon();
        icon.color = cardHandler.getAccentColor();
        bandeau.color = cardHandler.getAccentColor();

        return this;
    }

    public void Hide()
    {
        Debug.Log(rectTransform.anchoredPosition);
        //rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -500f);
        Debug.Log(rectTransform.anchoredPosition);
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
        siblingIndex = transform.GetSiblingIndex();
    }

    public void OnDrag(PointerEventData data)
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        Debug.Log("Pointer up");
        CardHolder cardHolder = Player.getSelectedCardHolder();
        Debug.Log(cardHolder);
        if (cardHolder == null)
        {
            rectTransform.SetParent(slot);
            rectTransform.anchorMin = 0.5f * Vector2.one;
            rectTransform.anchorMax = 0.5f * Vector2.one;
            rectTransform.anchoredPosition = startPos;
            transform.SetSiblingIndex(siblingIndex);
            Player.removeCard();
        }
        else
        {
            rectTransform.SetParent(cardHolder.rectTransform);
            rectTransform.anchorMin = 0.5f * Vector2.one;
            rectTransform.anchorMax = 0.5f * Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            Player.placeCard();
        }
        if (Player.getSelectedCard() == this) Player.setSelectedCard(null);
    }

    private void OnDestroy()
    {
        Destroy(slot.gameObject);
    }
}
 
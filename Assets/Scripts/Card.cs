using System;
using System.Collections;
using System.Collections.Generic;
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
    private Transform cardsLayout;
    private Vector2 startPos;
    private CardHandler cardHandler;
    private int siblingIndex;

    public void setup(string key, Transform cardsLayout)
    {
        cardHandler = DataManager.dictKeyToCard[key];
        titleDisplay.SetText(cardHandler.getKey());
        this.cardsLayout = cardsLayout;
        startPos = rectTransform.anchoredPosition;
        icon.sprite = cardHandler.getIcon();
        icon.color = cardHandler.getAccentColor();
        bandeau.color = cardHandler.getAccentColor();
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
            rectTransform.SetParent(cardsLayout);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
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
}
 
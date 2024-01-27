using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI titleDisplay;
    [SerializeField] private Image image;
    private Transform cardsLayout;
    private Vector2 startPos;
    private CardHandler cardHandler;

    public void setup(string key, Transform cardsLayout)
    {
        cardHandler = DataManager.dictKeyToCard[key];
        titleDisplay.SetText(cardHandler.getKey());
        this.cardsLayout = cardsLayout;
        startPos = rectTransform.anchoredPosition;
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
            rectTransform.anchoredPosition = startPos;
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
 
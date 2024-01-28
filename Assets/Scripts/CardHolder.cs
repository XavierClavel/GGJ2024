using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Card hoverCard;
    [HideInInspector] public Card selectedCard;
    public RectTransform rectTransform;
    
    public void OnPointerEnter(PointerEventData e)
    {
        Debug.Log("Mouse enter");
        Card card = Player.getSelectedCard();
        if (card == null) return;
        hoverCard = card;
        hoverCard.hoverCardHolder = this;
        if(selectedCard == null || hoverCard == selectedCard) hoverCard.rectTransform.DOScale(0.7f, 0.5f).SetEase(Ease.InOutQuad);
        //Player.setSelectedCardHolder(this);
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (hoverCard == null) return;
        if (selectedCard == null || selectedCard == hoverCard) hoverCard.rectTransform.DOScale(1f, 0.5f).SetEase(Ease.InOutQuad);
        hoverCard.hoverCardHolder = null;
        hoverCard = null;
        Debug.Log("Mouse exit");
        //if (Player.getSelectedCardHolder() == this) Player.setSelectedCardHolder(null);
    }

    public void UseCard()
    {
        if (selectedCard == null) return;
        Destroy(selectedCard.gameObject);
        selectedCard = null;
    }

    public bool isFree(Card card)
    {
        return selectedCard == null || selectedCard == card;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform rectTransform;
    public int index;
    public void OnPointerEnter(PointerEventData e)
    {
        Debug.Log("Mouse enter");
        Player.setSelectedCardHolder(this);
        Player.getSelectedCard()?.rectTransform.DOScale(0.7f, 0.5f).SetEase(Ease.InOutQuad);
    }

    public void OnPointerExit(PointerEventData e)
    {
        Debug.Log("Mouse exit");
        Player.getSelectedCard()?.rectTransform.DOScale(1f, 0.5f).SetEase(Ease.InOutQuad);
        if (Player.getSelectedCardHolder() == this) Player.setSelectedCardHolder(null);
    }

    public Vector3 getPosition()
    {
        return rectTransform.anchoredPosition;
    }

    private void OnMouseUp()
    {
        Debug.Log("Mouse Up");
        
    }

}

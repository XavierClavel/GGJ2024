using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    public void OnPointerExit(PointerEventData e)
    {
        Debug.Log("Mouse exit");
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

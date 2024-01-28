using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHolder : DraggableHolder<CardHandler>
{


    protected override void onPointerEnter()
    {
        if(selectedDraggable == null || hoverDraggable == selectedDraggable) hoverDraggable.rectTransform.DOScale(0.7f, 0.5f).SetEase(Ease.InOutQuad);
    }

    protected override Draggable<CardHandler> getSelectedDraggable()
    {
        return Player.getSelectedCard();
    }

    protected override void onPointerExit()
    {
        if (selectedDraggable == null || selectedDraggable == hoverDraggable) hoverDraggable.rectTransform.DOScale(1f, 0.5f).SetEase(Ease.InOutQuad);
    }

}

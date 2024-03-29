using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHolder : DraggableHolder
{

    protected override void onPointerEnter()
    {
        if(selectedDraggable == null || hoverDraggable == selectedDraggable) hoverDraggable.rectTransform.DOScale(0.66f, 0.5f).SetEase(Ease.InOutQuad);
    }

    protected override Draggable getSelectedDraggable()
    {
        return Player.getSelectedCard();
    }

    protected override void onPointerExit()
    {
        if (selectedDraggable == null || selectedDraggable == hoverDraggable) hoverDraggable.rectTransform.DOScale(1f, 0.5f).SetEase(Ease.InOutQuad);
    }

}

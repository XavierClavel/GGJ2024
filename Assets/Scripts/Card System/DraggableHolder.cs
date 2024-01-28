using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DraggableHolder<T> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Draggable<T> hoverDraggable;
    [HideInInspector] public Draggable<T> selectedDraggable;
    public RectTransform rectTransform;
    
    public void OnPointerEnter(PointerEventData e)
    {
        Debug.Log("Mouse enter");
        Draggable<T> draggable = getSelectedDraggable();
        if (draggable == null) return;
        hoverDraggable = draggable;
        hoverDraggable.hoverDraggableHolder = this;
        onPointerEnter();
    }

    protected abstract Draggable<T> getSelectedDraggable();
    
    public void OnPointerExit(PointerEventData e)
    {
        if (hoverDraggable == null) return;
        onPointerExit();
        hoverDraggable.hoverDraggableHolder = null;
        hoverDraggable = null;
        Debug.Log("Mouse exit");
        //if (Player.getSelectedCardHolder() == this) Player.setSelectedCardHolder(null);
    }

    protected virtual void onPointerEnter()
    {
        
    }

    protected virtual void onPointerExit()
    {
        
    }
    
    public void UseDraggable()
    {
        onUseDraggable();
        if (selectedDraggable == null) return;
        Destroy(selectedDraggable.gameObject);
        selectedDraggable = null;
    }

    public virtual void onUseDraggable()
    {
        
    }
    
    public bool isFree(Draggable<T> draggable)
    {
        return selectedDraggable == null || selectedDraggable == draggable;
    }

}

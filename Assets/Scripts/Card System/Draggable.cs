using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class Draggable<T> : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] protected Image image;
    public RectTransform rectTransform;
    protected Transform slot;
    [HideInInspector] public DraggableHolder<T> hoverDraggableHolder = null;
    [HideInInspector] public DraggableHolder<T> selectedDraggableHolder = null;
    protected bool canBeDragged = true;
    
    public void OnBeginDrag(PointerEventData data)
    {
        if (!canBeDragged) return;
        image.raycastTarget = false;
        Debug.Log("Pointer down");
        transform.SetParent(EnnemyManager.instance.canvas);
        onBeginDrag();
    }

    protected virtual void onBeginDrag()
    {
        
    }
    
    
    public void OnDrag(PointerEventData data)
    {
        if (!canBeDragged) return;
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canBeDragged) return;
        image.raycastTarget = true;
        
        if (hoverDraggableHolder == null)
        {
            AttachToSlot();
        } else if (!hoverDraggableHolder.isFree(this))
        {
            AttachToSlot();
            hoverDraggableHolder.hoverDraggable = null;
            hoverDraggableHolder = null;
        }
        else
        {
            if (selectedDraggableHolder != null && selectedDraggableHolder != hoverDraggableHolder)
            {
                selectedDraggableHolder.selectedDraggable = null;
                selectedDraggableHolder = null;
            }
            AttachToDraggableHolder(hoverDraggableHolder);
            selectedDraggableHolder = hoverDraggableHolder;
            selectedDraggableHolder.selectedDraggable = this;
            hoverDraggableHolder.hoverDraggable = null;
            hoverDraggableHolder = null;
        }
        
        onEndDrag();
    }

    protected virtual void onEndDrag()
    {
    }
    
    protected void AttachToDraggableHolder(DraggableHolder<T> draggableHolder)
    {
        rectTransform.SetParent(draggableHolder.rectTransform);
        rectTransform.anchorMin = 0.5f * Vector2.one;
        rectTransform.anchorMax = 0.5f * Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
    }
    
    protected void AttachToSlot()
    {
        if (selectedDraggableHolder != null)
        {
            selectedDraggableHolder.selectedDraggable = null;
            selectedDraggableHolder = null;
        }
        rectTransform.SetParent(slot);
        rectTransform.anchorMin = 0.5f * Vector2.one;
        rectTransform.anchorMax = 0.5f * Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
    }
}

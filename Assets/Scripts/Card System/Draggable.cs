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
    
    public void OnBeginDrag(PointerEventData data)
    {
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
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
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
}

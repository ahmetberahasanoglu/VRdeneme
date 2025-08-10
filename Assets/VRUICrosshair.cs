using UnityEngine;
using UnityEngine.EventSystems;

public class VRUICrosshair : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public fokometre foko;

    public void OnPointerDown(PointerEventData eventData)
    {
        foko.BeginDrag(eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foko.EndDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        foko.Dragging(eventData.pointerCurrentRaycast.worldPosition);
    }
}

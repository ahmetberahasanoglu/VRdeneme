using UnityEngine;
using UnityEngine.EventSystems;

public class VRUICrosshair : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public fokometre foko;

    public void OnPointerDown(PointerEventData eventData)
    {
        foko.OnBeginDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      
        foko.OnEndDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        foko.OnDrag(eventData);
    }
}

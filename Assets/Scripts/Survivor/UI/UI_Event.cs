using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Event : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> OnClickHandler;
    public Action<PointerEventData> OnDragHandler;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(OnClickHandler != null)
        {
            OnClickHandler.Invoke(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
        {
            OnDragHandler.Invoke(eventData);
        }
    }
}

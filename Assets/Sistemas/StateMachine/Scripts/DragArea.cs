using UnityEngine;
using UnityEngine.EventSystems;

namespace StateMachine
{
    public class DragArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] StateMachineDisplay display;

        public void OnBeginDrag(PointerEventData eventData) => display.OnBeginDrag(eventData);

        public void OnDrag(PointerEventData eventData) => display.OnDrag(eventData);

        public void OnEndDrag(PointerEventData eventData) => display.OnEndDrag(eventData);
    }
}
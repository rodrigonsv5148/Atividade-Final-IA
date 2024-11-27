using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundTargetPosition : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] SteeringAgent agent; // Referência ao agente
    [SerializeField] Transform target;   // Referência ao objeto alvo

    private bool isMousePressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Atualiza a posição do alvo com base no clique inicial
        UpdateTargetPosition(eventData);
        isMousePressed = true;
        agent.Target = target;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Quando o botão do mouse é solto, paramos de atualizar a posição
        isMousePressed = false;
    }

    private void Update()
    {
        // Se o botão do mouse estiver pressionado, atualiza a posição do alvo continuamente
        if (isMousePressed)
        {
            // Usa o raycast do mouse para determinar a posição no mundo
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var raycastResults = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            if (raycastResults.Count > 0)
            {
                target.position = raycastResults[0].worldPosition;
            }
        }
    }

    private void UpdateTargetPosition(PointerEventData eventData)
    {
        // Atualiza a posição do alvo com base no evento recebido
        target.position = eventData.pointerPressRaycast.worldPosition;
    }
}

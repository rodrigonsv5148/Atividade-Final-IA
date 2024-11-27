using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundTargetPosition : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] SteeringAgent agent; // Refer�ncia ao agente
    [SerializeField] Transform target;   // Refer�ncia ao objeto alvo

    private bool isMousePressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Atualiza a posi��o do alvo com base no clique inicial
        UpdateTargetPosition(eventData);
        isMousePressed = true;
        agent.Target = target;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Quando o bot�o do mouse � solto, paramos de atualizar a posi��o
        isMousePressed = false;
    }

    private void Update()
    {
        // Se o bot�o do mouse estiver pressionado, atualiza a posi��o do alvo continuamente
        if (isMousePressed)
        {
            // Usa o raycast do mouse para determinar a posi��o no mundo
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
        // Atualiza a posi��o do alvo com base no evento recebido
        target.position = eventData.pointerPressRaycast.worldPosition;
    }
}

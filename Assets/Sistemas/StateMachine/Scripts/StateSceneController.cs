using StateMachine;
using UnityEngine;

public class StateSceneController : MonoBehaviour
{
    [SerializeField] StateMachineController controller;
    [SerializeField] DirectionalInput directional;
    [SerializeField] OnScreenButtons buttons;
    [SerializeField] CanvasGroup playCanvas;
    [SerializeField] CanvasGroup configureCanvas;
    [SerializeField] GameObject character;
    SceneState state = SceneState.Configuring;

    public enum SceneState { Configuring, Playing }

    public void ToggleState()
    {
        state = state switch
        {
            SceneState.Configuring => SceneState.Playing,
            SceneState.Playing => SceneState.Configuring,
            _ => SceneState.Configuring,
        };
        playCanvas.blocksRaycasts = state == SceneState.Playing ? true : false;
        playCanvas.alpha = state == SceneState.Playing ? 1 : 0;
        character.gameObject.SetActive(state == SceneState.Playing);
        configureCanvas.blocksRaycasts = state == SceneState.Configuring ? true : false;
        configureCanvas.alpha = state == SceneState.Configuring ? 1 : 0;
        // playCanvas.gameObject.SetActive(state == SceneState.Playing);
        // configureCanvas.gameObject.SetActive(state == SceneState.Configuring);
    }

    void Update()
    {
        if (state != SceneState.Playing)
            return;

        var input = new StateMachine.Input()
        {
            Horizontal = directional.Latest.x,
            Vertical = directional.Latest.y,
            X = buttons.X,
            Y = buttons.Y,
            A = buttons.A,
            B = buttons.B
        };

        controller.ProcessInput(input);
    }
}

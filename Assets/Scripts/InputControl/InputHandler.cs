using UnityEngine;

namespace InputControl
{

    public class InputHandler : MonoBehaviour
    {
        public static InputStateWaiting stateWaiting;
        public static InputStateMoveCamera stateMoveCamera;
        public static InputStatePlayGame statePlayGame;
        public static InputStateZoom stateZoom;
        private IInputState state;

        private void Start()
        {
            // init all ins state
            stateWaiting = new InputStateWaiting();
            stateMoveCamera = new InputStateMoveCamera();
            statePlayGame = new InputStatePlayGame();
            stateZoom = new InputStateZoom();
            
            state = new InputStateWaiting();
        }

        private void Update()
        {
            var newState = state.Update();
            if (newState != null)
            {
                state = newState;
                state.OnEnterState();
            }
        }
    }
}
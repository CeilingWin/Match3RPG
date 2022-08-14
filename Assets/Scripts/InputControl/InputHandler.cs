using UnityEngine;

namespace InputControl
{

    public class InputHandler : MonoBehaviour
    {
        private IInputState state;

        private void Start()
        {
            state = new InputStateWaiting();
        }

        private void Update()
        {
            var newState = state.Update();
            if (newState != null) state = newState;
        }
    }
}
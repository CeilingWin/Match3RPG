using UnityEngine;

namespace InputControl
{
    public class InputStatePlayGame : IInputState
    {
        private Vector3 startPos;

        public InputStatePlayGame()
        {
        }

        public void OnEnterState()
        {
            var touch = Input.touches[0];
            var ray = Camera.main.ScreenPointToRay(touch.position);
            Physics.Raycast(ray, out var target, float.PositiveInfinity, 1 << 4);
            startPos = target.transform.position;
        }

        public IInputState Update()
        {
            return InputHandler.stateWaiting;
        }
    }
}
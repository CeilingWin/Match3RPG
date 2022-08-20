using UnityEngine;

namespace InputControl
{
    public class InputStateZoom : IInputState
    {
        private float defaultFieldOfView;
        private const float MaxZoom = 3f;
        private const float MinZoom = 0.77f;

        private float zoomLevel;
        private float lastDistance;

        private InputHandler inputHandler;
        public InputStateZoom(InputHandler handler)
        {
            inputHandler = handler;
            defaultFieldOfView = Camera.main!.fieldOfView;
            zoomLevel = 1;
        }

        public void OnEnterState()
        {
            lastDistance = GetTouchesDistance();
        }

        private float GetTouchesDistance()
        {
            var touches = Input.touches;
            return Vector2.Distance(touches[0].position, touches[1].position);
        }

        public IInputState Update()
        {
            var numTouch = Input.touches.Length;
            if (numTouch == 0) return InputHandler.stateWaiting;
            if (numTouch == 1) return InputHandler.stateMoveCamera;
            var currentDistance = GetTouchesDistance();
            var ratio = currentDistance / lastDistance;
            zoomLevel *= (ratio + (1 - ratio) / 5f);
            if (zoomLevel < MinZoom) zoomLevel = MinZoom;
            if (zoomLevel > MaxZoom) zoomLevel = MaxZoom;
            UpdateZoom();
            lastDistance = currentDistance;
            return null;
        }

        private void UpdateZoom()
        {
            Camera camera = Camera.current;
            var fieldOfView = defaultFieldOfView / zoomLevel;
            camera.fieldOfView = fieldOfView;
        }
    }
}
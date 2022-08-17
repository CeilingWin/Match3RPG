using UnityEngine;

namespace InputControl
{
    public class InputStateMoveCamera : IInputState
    {
        private static float SPEED = 10f;
        private Vector2 lastPoint;
        public InputStateMoveCamera()
        {
        }

        public void OnEnterState()
        {
            lastPoint = Input.touches[0].position;
        }

        public IInputState Update()
        {
            var numTouch = Input.touches.Length;
            if (numTouch == 1)
            {
                HandleMoveCamera();
                return null;
            }
            if (numTouch == 2) return InputHandler.stateZoom;
            return InputHandler.stateWaiting;
        }

        private void HandleMoveCamera()
        {
            var currentPoint = Input.touches[0].position;
            var deltaX = currentPoint.x - lastPoint.x;
            float angle = deltaX * Time.deltaTime * InputStateMoveCamera.SPEED;
            if (currentPoint.y > Screen.height / 2f) angle = -angle;
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, angle);
            lastPoint = currentPoint; 
        }
    }
}
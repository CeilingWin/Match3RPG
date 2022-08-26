using System;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace InputControl
{
    public class InputStateMoveCamera : IInputState
    {
        private static float SPEED = 10f;
        private Vector2 lastPoint;
        private InputHandler inputHandler;
        public InputStateMoveCamera(InputHandler handler)
        {
            inputHandler = handler;
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
            var deltaY = currentPoint.y - lastPoint.y;
            float delta, midPivot, currentP, sign;
            if (Math.Abs(deltaX) > Math.Abs(deltaY))
            {
                delta = deltaX;
                midPivot = Screen.height / 2f;
                currentP = currentPoint.y;
                sign = 1f;
            }
            else
            {
                delta = deltaY;
                midPivot = Screen.width / 2f;
                currentP = currentPoint.x;
                sign = -1f;
            }
            float angle = sign * delta * Time.deltaTime * InputStateMoveCamera.SPEED;
            if (currentP > midPivot) angle = -angle;
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, angle);
            lastPoint = currentPoint; 
        }
    }
}
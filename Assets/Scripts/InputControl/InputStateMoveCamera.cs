using UnityEngine;

namespace InputControl
{
    public class InputStateMoveCamera : IInputState
    {
        private Vector2 lastPoint;
        public InputStateMoveCamera()
        {
            Debug.Log("move camera");
            lastPoint = Input.touches[0].position;
        }
        public IInputState Update()
        {
            var numTouch = Input.touches.Length;
            if (numTouch == 1)
            {
                HandleMoveCamera();
            }
            else
            {
                if (numTouch == 0) return new InputStateWaiting();
                if (numTouch == 2) return new InputStateZoom();
            }
            return null;
        }

        private void HandleMoveCamera()
        {
            var currentPoint = Input.touches[0].position;
            var deltaX = currentPoint.x - lastPoint.x;
            float angle = deltaX * Time.deltaTime * 10;
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, angle);
            lastPoint = currentPoint;
        }
    }
}
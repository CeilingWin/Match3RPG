using System;
using DefaultNamespace;
using UnityEngine;

namespace InputControl
{
    public class InputStatePlayGame : IInputState
    {
        private Vector3 startPos;

        private readonly InputHandler inputHandler;
        public InputStatePlayGame(InputHandler handler)
        {
            inputHandler = handler;
        }

        public void OnEnterState()
        {
            var touch = Input.touches[0];
            startPos = ScreenPointToGamePoint(touch.position);
            inputHandler.OnTouchBegan(startPos);
        }

        public IInputState Update()
        {
            var numTouch = Input.touches.Length;
            if (numTouch != 1)
            {
                inputHandler.OnTouchCanceled();
                return InputHandler.stateWaiting;
            }

            var touch = Input.touches[0];
            try
            {
                var position = ScreenPointToGamePoint(touch.position);
                if (touch.phase == TouchPhase.Moved)
                {
                    inputHandler.OnTouchMoved(position);
                } else if (touch.phase == TouchPhase.Ended)
                {
                    inputHandler.OnTouchEnded(position);
                } else if (touch.phase == TouchPhase.Canceled)
                {
                    inputHandler.OnTouchCanceled();
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                inputHandler.OnTouchCanceled();
                return InputHandler.stateWaiting;
            }
        }

        private Vector3 ScreenPointToGamePoint(Vector3 touchPosition)
        {
            var ray = InputHandler.MainCamera.ScreenPointToRay(touchPosition);
            var isHit = Physics.Raycast(ray, out var target, float.PositiveInfinity, GameConst.CheckTouchMark);
            if (!isHit) throw new Exception("Invalid touched");
            return target.point;
        }
    }
}
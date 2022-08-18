using DefaultNamespace;
using UnityEngine;

namespace InputControl
{
    public class InputStateWaiting : IInputState
    {
        private InputHandler inputHandler;
        public InputStateWaiting(InputHandler handler)
        {
            inputHandler = handler;
        }
        public void OnEnterState()
        {
            
        }

        public IInputState Update()
        {
            if (Input.touches.Length == 1)
            {
                var touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began)
                {
                    var ray = InputHandler.MainCamera.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out var target, float.PositiveInfinity, GameConst.CheckTouchMark))
                    {
                        if (target.transform.CompareTag(GameConst.TagGameBoard))
                        {
                            return InputHandler.statePlayGame;
                        }
                    }
                    return InputHandler.stateMoveCamera;
                }
            } else if (Input.touches.Length == 2)
            {
                return InputHandler.stateZoom;
            }
            return null;
        }
    }
}
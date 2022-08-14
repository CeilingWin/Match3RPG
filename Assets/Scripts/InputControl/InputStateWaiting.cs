using UnityEngine;

namespace InputControl
{
    public class InputStateWaiting : IInputState
    {
        public IInputState Update()
        {
            if (Input.touches.Length == 1)
            {
                var touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began)
                {
                    var ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out var target, float.PositiveInfinity, 1<<4))
                    {
                        if (target.transform.CompareTag("Item"))
                        {
                            return new InputStatePlayGame(target.transform.position);
                        }
                        return new InputStateMoveCamera();
                    }
                }
            } else if (Input.touches.Length == 2)
            {
                return new InputStateZoom();
            }
            return null;
        }
    }
}
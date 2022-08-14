using UnityEngine;

namespace InputControl
{
    public interface IInputHandlerControllable
    {
        void OnTouchBegan(Vector3 pointer);
        void OnTouchEnded(Vector3 pointer);
        void OnTouchCancel(Vector3 pointer);
        void OnClick(Vector3 pointer);
    }
}
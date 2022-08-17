using Unity.VisualScripting;
using UnityEngine;

namespace InputControl
{
    public interface IInputState
    {
        public void OnEnterState();
        public IInputState Update();
    }
}
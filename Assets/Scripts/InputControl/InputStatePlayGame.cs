using UnityEngine;

namespace InputControl
{
    public class InputStatePlayGame : IInputState
    {
        private Vector3 startPos;
        public InputStatePlayGame(Vector3 startPos)
        {
            this.startPos = startPos;
        }
        public IInputState Update()
        {
            throw new System.NotImplementedException();
        }
    }
}
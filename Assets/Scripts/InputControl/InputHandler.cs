using System;
using UnityEngine;

namespace InputControl
{

    public class InputHandler : MonoBehaviour
    {
        public static Camera MainCamera;
        public static InputStateWaiting stateWaiting;
        public static InputStateMoveCamera stateMoveCamera;
        public static InputStatePlayGame statePlayGame;
        public static InputStateZoom stateZoom;
        private IInputState state;

        public event EventHandler<Vector3> TouchBegan;
        public event EventHandler<Vector3> TouchMoved;
        public event EventHandler<Vector3> TouchEnded;
        public event EventHandler TouchCanceled;
        private void Start()
        {
            MainCamera = Camera.main;
            // init all ins state
            stateWaiting = new InputStateWaiting(this);
            stateMoveCamera = new InputStateMoveCamera(this);
            statePlayGame = new InputStatePlayGame(this);
            stateZoom = new InputStateZoom(this);

            state = stateWaiting;
        }

        private void Update()
        {
            var newState = state.Update();
            if (newState != null)
            {
                state = newState;
                state.OnEnterState();
            }
            
            // todo: continue invoke event play game
        }

        public void OnTouchCanceled()
        {
            // Debug.Log("on touch cancel");
            TouchCanceled?.Invoke(this, EventArgs.Empty);
        }

        public void OnTouchBegan(Vector3 e)
        {
            // Debug.Log("on touch began");
            TouchBegan?.Invoke(this, e);
        }

        public void OnTouchMoved(Vector3 e)
        {
            // Debug.Log("on touch moved");
            TouchMoved?.Invoke(this, e);
        }

        public void OnTouchEnded(Vector3 e)
        {
            // Debug.Log("on touch ended");
            TouchEnded?.Invoke(this, e);
        }
    }
}
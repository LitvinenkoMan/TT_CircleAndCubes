using System.Collections.Generic;
using System.Threading;
using _Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Mono
{
    public class PlayerMovement : MonoBehaviour, MainInputActions.IPlayerActions, IMovable
    {
        [SerializeField] private float SingleTapMovementTime;
        [SerializeField] private float HoldMovementTime;

        private CancellationTokenSource _cts;
        private List<Vector2> _touches;
        private MainInputActions _input;

        private bool _recordLine;

        void Start()
        {
            _input = new MainInputActions();
            _input.Enable();
            _input.Player.SetCallbacks(this);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                Debug.Log("Performed Click");
            }
        }

        public void OnHold(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                Debug.Log("Performed Hold");
            }
        }

        public void OnHoldRelease(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Performed HoldRelease");
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Performed Move");
            }
        }

        public UniTask MoveToPosition(Vector2 pos, float moveTime)
        {
            return UniTask.CompletedTask;
        }

        public UniTask MoveAlongTheLine(List<Vector2> line, float moveTime)
        {
            return UniTask.CompletedTask;
        }
    }
}

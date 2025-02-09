using System;
using System.Collections.Generic;
using System.Threading;
using _Scripts.Interfaces;
using _Scripts.ScriptableObject;
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
        private MainInputActions _input;
        private List<Vector2> _touches;
        private Camera _camera;

        private Vector2 _currentPointerPosition;
        private bool _recordLine;
        private float _distance;

        void Start()
        {
            _input = new MainInputActions();
            _input.Enable();
            _input.Player.SetCallbacks(this);

            _camera = Camera.main;
        }

        public async void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var worldPos =
                    _camera.ScreenToWorldPoint(new Vector3(_currentPointerPosition.x, _currentPointerPosition.y, 0));
                await MoveToPosition(worldPos, SingleTapMovementTime);
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
                _currentPointerPosition = context.ReadValue<Vector2>();
            }
        }

        public async UniTask MoveToPosition(Vector2 pos, float moveTime)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            
            Vector2 startPos = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < moveTime)
            {
                if (_cts.Token.IsCancellationRequested) return;

                elapsedTime += Time.deltaTime;
                float t = elapsedTime / moveTime;

                float easedT = t * t * (3f - 2f * t);

                transform.position = Vector2.Lerp(startPos, pos, easedT);
                
                await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
            }

            transform.position = pos;
        }

        public UniTask MoveAlongTheLine(List<Vector2> line, float moveTime)
        {
            return UniTask.CompletedTask;
        }
    }
}

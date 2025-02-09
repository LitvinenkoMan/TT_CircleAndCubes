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
        private Queue<Vector2> _pointsList;
        private CircleCollider2D _collider;
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
            _pointsList = new Queue<Vector2>();
            _collider = GetComponent<CircleCollider2D>();
        }

        public async void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var worldPos =
                    _camera.ScreenToWorldPoint(new Vector3(
                        _currentPointerPosition.x,
                        _currentPointerPosition.y,
                        _camera.gameObject.transform.position.z));              // this method returns position relative from camera
                                                                                // that means I need to add camera position to start from Vector.zero 
                if (Vector3.Distance(worldPos, transform.position) < _collider.radius)
                {
                    _cts?.Cancel();
                }
                else await MoveToPosition(worldPos, SingleTapMovementTime);
                
                Debug.Log("Performed Click");
            }
        }

        public void OnHold(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Performed Hold");
                _recordLine = true;
            }
        }

        public async void OnHoldRelease(InputAction.CallbackContext context)
        {
            if (context.performed && _recordLine)
            {
                Debug.Log("Performed HoldRelease");
                _recordLine = false;
                await MoveAlongTheLine(_pointsList, HoldMovementTime);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _currentPointerPosition = context.ReadValue<Vector2>();
                if (_recordLine)
                {
                    var worldPos =
                        _camera.ScreenToWorldPoint(new Vector3(_currentPointerPosition.x, _currentPointerPosition.y, 0));
                    _pointsList.Enqueue(worldPos);
                }
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

        public async UniTask MoveAlongTheLine(Queue<Vector2> line, float moveTime)
        {
            // just using the same method, but with Queue
            while (line.Count > 0)
            {
                if (_cts.Token.IsCancellationRequested)
                {
                    line.Clear();
                    return;
                }
                await MoveToPosition(line.Dequeue(), moveTime / line.Count);
            }
        }
    }
}
